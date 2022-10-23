using LibraryAPI.Exceptions;
using LibraryAPI.Interfaces;
using LibraryAPI.Models;
using LibraryAPI.Models.Dto;
using LibraryAPI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuditService _auditService;

        public AuthService(
            ILogger<AuthService> logger,
            AppDbContext context,
            IConfiguration configuration,
            IPasswordHasher<User> passwordHasher,
            IAuditService auditService)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _auditService = auditService;
        }


        public object Login(LoginDto dto)
        {
            var user = _context.Users
              .AsNoTracking()
              .FirstOrDefault(x => x.Username == dto.Username);

            if (user is null)
            {
                _auditService.SecurityAuditUserLoginAttemptFails(dto.Username, $" => not exits");
                throw new AuthException("Login => user not exits");
            }

            if (user.IsEnabled == false)
            {
                _auditService.SecurityAuditUserLoginAttemptFails(dto.Username, $" => is closed");
                throw new AuthException("Login => user not exits");
            }

            if (user.IsLocked == true)
            {
                _auditService.SecurityAuditUserLoginAttemptFails(user.Username, $" => is locked");
                throw new AuthException("Login => account is locked. Contact with library employee");
            }

            if (user.IsConfirmed == false)
            {
                _auditService.SecurityAuditUserLoginAttemptFails(user.Username, $" => not confirmed");
                throw new AuthException("Login => account is not confirmed");
            }

            // Check password hash

            var userHashPasswords = _context.UserCredentials
              .AsNoTracking()
              .FirstOrDefault(x => x.Id == user.CurrUserCredentialId);

            if (userHashPasswords is null)
            {
                _auditService.SecurityAuditUserLoginAttemptFails(user.Id, user.Username, $" => password is null");
                throw new AuthException("Login => username or password invalid");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, userHashPasswords.Password, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                _auditService.SecurityAuditUserLoginAttemptFails(user.Id, user.Username, $" => incorrect password");
                throw new AuthException("Login => username or password invalid");
            }

            // Auth correct

            // Todo ...

            _auditService.SecurityAuditUserLoginAttemptSuccess(user.Id, user.Username);

            return user.Id;
        }

        public Task<object> Logout()
        {
            throw new NotImplementedException();
        }

 
    }
}