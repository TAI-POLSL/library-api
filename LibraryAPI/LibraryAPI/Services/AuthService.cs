using LibraryAPI.Exceptions;
using LibraryAPI.Interfaces;
using LibraryAPI.Models;
using LibraryAPI.Models.Dto;
using LibraryAPI.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuditService _auditService;
        private readonly IHeaderContextService _headerContextService;

        public AuthService(
            ILogger<AuthService> logger,
            AppDbContext context,
            IConfiguration configuration,
            IPasswordHasher<User> passwordHasher,
            IAuditService auditService,
            IHeaderContextService headerContextService
            )
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _auditService = auditService;
            _headerContextService = headerContextService;
        }


        public async Task<object> LoginAsync(LoginDto dto)
        {
            if (_headerContextService.IsAuthenticated() == true)
            {
                throw new BadHttpRequestException("Login => session cookie exists");
            }

            var user = _context.Users
                .AsNoTracking()
                .FirstOrDefault(x => x.Username == dto.Username);

            if (user is null)
            {
                _auditService.SecurityAuditUserLoginAttemptFails(null, dto.Username, $"=> user not exits");
                _logger.LogError("Login => user not exits");
                throw new AuthException("Login => user not exits");
            }

            if (user.IsEnabled == false)
            {
                _auditService.SecurityAuditUserLoginAttemptFails(user.Id, dto.Username, $"=> is closed");
                throw new AuthException("Login => user not exits");
            }

            if (user.IsLocked == true)
            {
                _auditService.SecurityAuditUserLoginAttemptFails(user.Id, user.Username, $"=> is locked");
                throw new AuthException("Login => account is locked. Contact with library employee");
            }

            if (user.IsConfirmed == false)
            {
                _auditService.SecurityAuditUserLoginAttemptFails(user.Id, user.Username, $"=> not confirmed");
                throw new AuthException("Login => account is not confirmed");
            }

            // Check password hash

            var userHashPasswords = _context.UserCredentials
              .AsNoTracking()
              .FirstOrDefault(x => x.Id == user.CurrUserCredentialId);

            if (userHashPasswords is null)
            {
                _auditService.SecurityAuditUserLoginAttemptFails(user.Id, user.Username, $"=> password is null");
                throw new AuthException("Login => username or password invalid");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, userHashPasswords.Password, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                _auditService.SecurityAuditUserLoginAttemptFails(user.Id, user.Username, $"=> incorrect password");
                throw new AuthException("Login => username or password invalid");
            }

            // Create SESSION in db

            var session = new Session()
            {
                UserId = user.Id,
                IpAddress = _headerContextService.GetUserRemoteIpAddress(),
            };

            _context.Sessions.Add(session);
            _context.SaveChanges();

            // Generate SESSION cookie with claims

            var claims = new List<Claim>
            {
                new Claim("SessionID", session.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            await _headerContextService.GetHttpContext().SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(15),
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                });

            _auditService.SecurityAuditUserLoginAttemptSuccess(user.Id, user.Username);

            return user.Role;
        }

        public async Task<object> Logout()
        {
            if (_headerContextService.IsAuthenticated() == false)
            {
                return "";
            }

            Guid sessionId = _headerContextService.GetUserSessionId();
            Guid userId = _headerContextService.GetUserId();
            var username = _headerContextService.GetUserUsername();

            var session = _context.Sessions.Where(x => x.Id == sessionId).First();
            _context.Sessions.Remove(session);

            _auditService.SecurityAuditUserLogoutAttemptSuccess(userId, username);

            await _headerContextService.GetHttpContext().SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _context.SaveChanges();

            return "";
        }
    }
}