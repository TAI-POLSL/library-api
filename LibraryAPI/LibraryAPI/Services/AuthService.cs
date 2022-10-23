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
        protected readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHeaderContextService _headerContextService;

        public AuthService(
            ILogger<AuthService> logger,
            AppDbContext context,
            IConfiguration configuration,
            IPasswordHasher<User> passwordHasher,
            IAuditService auditService,
            IHttpContextAccessor httpContextAccessor,
            IHeaderContextService headerContextService
            )
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _auditService = auditService;
            _httpContextAccessor = httpContextAccessor;
            _headerContextService = headerContextService;
        }


        public async Task<object> LoginAsync(LoginDto dto)
        {
            var ss = _httpContextAccessor.HttpContext?.Request.Cookies["SESSION"];

            if (ss != null)
            {
                throw new AuthException("Login => session cookie exists");
            }

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

            // Create SESSION in db

            var session = new Session()
            {
                UserId = user.Id,
                IpAddress = _headerContextService.RemoteIpAddress(),
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

            await _httpContextAccessor.HttpContext!.SignInAsync(
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

            return user.Id;
        }

        public async Task<object> Logout()
        {
            if (_httpContextAccessor.HttpContext == null) {
                await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return "";
            }

            var sessionId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "SessionID")?.Value!;

            if (sessionId == null) {
                await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return "";
            }

            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            var username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value!;

            var guidSessionId = new Guid(sessionId);
            var guidUserGuidId = new Guid(userId);

            var session = _context.Sessions.Where(x => x.Id == guidSessionId).First();
            _context.Sessions.Remove(session);

            _auditService.SecurityAuditUserLogoutAttemptSuccess(guidUserGuidId, username);

            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _context.SaveChanges();

            return "";
        }
    }
}