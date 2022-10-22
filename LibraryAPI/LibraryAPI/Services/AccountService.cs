using LibraryAPI.Models.Dto;
using LibraryAPI.Interfaces;
using LibraryAPI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using LibraryAPI.Models;
using LibraryAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services
{
    public class AccountService : IAccountService
    {

        private readonly ILogger<AccountService> _logger;
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly IHeaderContextService _headerContextService;

        public AccountService(
            ILogger<AccountService> logger,
            AppDbContext context,
            IPasswordHasher<User> passwordHasher,
            IConfiguration configuration,
            IHeaderContextService headerContextService)
        {
            _logger = logger;
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _headerContextService = headerContextService;
        }


        public object ChangePassword(ChangePasswordDto dto)
        {
            throw new NotImplementedException();
        }

        public object Close(Guid userId)
        {
            throw new NotImplementedException();
        }

        public object Lock(Guid userId)
        {
            throw new NotImplementedException();
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
                }
            };

            var credential = new UserCredential()
            {
                Password = _passwordHasher.HashPassword(user, dto.Password),
                IP = _headerContextService.RemoteIpAddress()
            };

            user.UserCredentials.Add(credential);

            _context.Users.Add(user);
            _context.SaveChanges();

            return user.Id;
        }
    }
}
