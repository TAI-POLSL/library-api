using LibraryAPI.Models.Dto;
using LibraryAPI.Interfaces;
using LibraryAPI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using LibraryAPI.Models;
using LibraryAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Enums;

namespace LibraryAPI.Services
{
    public class AccountService : IAccountService
    {

        private readonly ILogger<AccountService> _logger;
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly IHeaderContextService _headerContextService;
        private readonly IAuditService _auditService;

        public AccountService(
            ILogger<AccountService> logger,
            AppDbContext context,
            IPasswordHasher<User> passwordHasher,
            IConfiguration configuration,
            IHeaderContextService headerContextService,
            IAuditService auditService)
        {
            _logger = logger;
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _headerContextService = headerContextService;
            _auditService = auditService;
        }

        public object Get(Guid? userId = null)
        {
            IQueryable<User> query = _context.Users
                .AsNoTracking()
                .Include(x => x.Person);

            if (userId != null)
            {
                query = query.Where(x => x.Id == userId);
            }

            var fetch = query.Select(x => new
            {
                x.Id,
                x.Username,
                x.Person.FirstName,
                x.Person.LastName,
                x.Person.FullName,
                x.Person.Email,
                x.Person.Address,
                x.Person.Gender
            });

            var result = fetch.ToList();

            return result;
        }

        public object GetAuditByUserId(Guid userId)
        {
            var query = _context.Audits
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new {
                    x.Id,
                    x.UserId,
                    x.Operation,
                    x.IP,
                    x.Time,
                    x.Description,
                });

            return query.ToList();
        }

        public async Task<object> ChangePassword(ChangePasswordDto dto, Guid userId)
        {
            if (dto.NewPassword != dto.ConfirmNewPassword)
            {
                throw new RegisterException("ChangePassword => Passwords not matching");
            }

            var user = _context.Users
                .Include(x => x.UserCredentials)
                .Where(x => x.IsEnabled == true && x.IsLocked == false && x.IsConfirmed == true)
                .FirstOrDefault(x => x.Id == userId);

            if (user is null)
            {
    
                _auditService.SecurityAudit(userId, Enums.SecurityOperation.USER_PASSWORD_CHANGE, $"no user account");
                throw new AuthException("ChangePassword => no user account");
            }

            var currentEntityPassword = user.UserCredentials
                .Where(x => x.Id == user.CurrUserCredentialId)
                .First();

            var result = _passwordHasher.VerifyHashedPassword(user, currentEntityPassword.Password, dto.OldPassword);

            if (result == PasswordVerificationResult.Failed)
            {
                _auditService.SecurityAudit(userId, Enums.SecurityOperation.USER_PASSWORD_CHANGE, $"{user.Username}: incorrect current auth password");
                throw new AuthException("ChangePassword => invalid password");
            }

            // Expired old passwords
            currentEntityPassword.ExpiredDate = DateTime.UtcNow;

            // Push new password
            var credential = new UserCredential()
            {
                User = user,
                Password = _passwordHasher.HashPassword(user, dto.NewPassword),
                IP = _headerContextService.RemoteIpAddress(),
            };

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async ()  =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.UserCredentials.Add(credential);
                        _context.SaveChanges();

                        user.UserCredentials.Add(credential);
                        var oldUserCredentialId = user.CurrUserCredentialId;
                        user.CurrUserCredentialId = credential.Id;

                        _context.Users.Update(user);
                        _context.SaveChanges();

                        _auditService.AuditDbTable(Guid.Empty, DbTables.USERS, user.Id.ToString(), DbOperations.UPDATE, $"CurrUserCredentialId from {oldUserCredentialId} to {credential.Id}");
                        _auditService.AuditDbTable(Guid.Empty, DbTables.USERS_CREDENTIALS, credential.Id.ToString(), DbOperations.INSERT, "");

                        _auditService.SecurityAudit(userId, Enums.SecurityOperation.USER_PASSWORD_CHANGE, $"{user.Username}: successfull password update");
                        
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _auditService.SecurityAudit(userId, Enums.SecurityOperation.USER_PASSWORD_CHANGE, $"{user.Username}: unsuccessfull password update");
                        throw new RegisterException(ex.Message);
                    }

                }
            });

            return 200;
        }

        public object Close(Guid userId)
        {
            var user = _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefault();

            if (user == null)
            {
                _auditService.SecurityAudit(userId, Enums.SecurityOperation.USER_ACCOUNT_DELETED, $"no user account");
                throw new NotFoundException("Close => User not found");
            }

            var old = user.IsEnabled;
            user.IsEnabled = false;

            _context.SaveChanges();

            _auditService.AuditDbTable(Guid.Empty, DbTables.USERS, user.Id.ToString(), DbOperations.UPDATE, $"IsEnable from {old} to {user.IsEnabled}");
            _auditService.SecurityAudit(userId, Enums.SecurityOperation.USER_ACCOUNT_DELETED, $"{user.Username}: account close successfull");

            return 200;
        }

        public object Lock(Guid userId)
        {
            var user = _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefault();

            if (user == null)
            {
                _auditService.SecurityAudit(userId, Enums.SecurityOperation.USER_ACCOUNT_LOCK, $"no user account");
                throw new NotFoundException("Lock => User not found");
            }

            var old = user.IsLocked;
            user.IsLocked = true;

            _context.SaveChanges();

            _auditService.AuditDbTable(Guid.Empty, DbTables.USERS, user.Id.ToString(), DbOperations.UPDATE, $"IsLocked from {old} to {user.IsLocked}");
            _auditService.SecurityAudit(userId, Enums.SecurityOperation.USER_ACCOUNT_LOCK, $"{user.Username}: account lock successfull");

            return 200;
        }

        public async Task<object> Register(RegisterDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                throw new RegisterException("Register => Passwords are different");
            }

            var ex = _context.Users
                .AsNoTracking()
                .Include(u => u.Person)
                .Where(u => u.Username == dto.Username || u.Person.Email == dto.Email)
                .Select(u => u.Id)
                .ToList();

            if (ex.Count > 0)
            {
                throw new RegisterException("Register => Username/Email not unique");
            }

            var user = new User()
            {
                Username = dto.Username,
                IsConfirmed = true,
                IsEnabled = true,
                Role = dto.Roles,
                Person = new Person()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Gender = dto.Gender,
                    Email = dto.Email,
                    StreetAddress = dto.StreetAddress,
                    PostalCode = dto.PostalCode,
                    City = dto.City,
                    State = dto.State
                },
                UserCredentials = new HashSet<UserCredential>()
            };

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Users.Add(user);

                        var credential = new UserCredential()
                        {
                            User = user,
                            Password = _passwordHasher.HashPassword(user, dto.Password),
                            IP = _headerContextService.RemoteIpAddress(),
                        };

                        _context.UserCredentials.Add(credential);

                        _context.SaveChanges();

                        user.CurrUserCredentialId = credential.Id;

                        _context.SaveChanges();

                        _auditService.AuditDbTable(user.Id, DbTables.USERS, user.Id.ToString(), DbOperations.INSERT, "");
                        _auditService.AuditDbTable(user.Id, DbTables.USERS_CREDENTIALS, credential.Id.ToString(), DbOperations.INSERT, "");

                        _auditService.SecurityAudit(user.Id, Enums.SecurityOperation.USER_ACCOUNT_CREATED, $"{user.Username}: account create successfull");

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _auditService.SecurityAudit(user.Id, Enums.SecurityOperation.USER_ACCOUNT_CREATED, $"{user.Username}: account not create successfull");
                        throw new RegisterException(ex.Message);
                    }

                }
            });

            return user.Id;
        }
    }
}
