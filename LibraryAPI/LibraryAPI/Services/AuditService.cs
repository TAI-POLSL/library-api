using LibraryAPI.Enums;
using LibraryAPI.Interfaces;
using LibraryAPI.Models;
using LibraryAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services
{
    public class AuditService : IAuditService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly AppDbContext _context;
        private readonly IHeaderContextService _headerContextService;

        public AuditService(
            ILogger<AuthService> logger,
            AppDbContext context,
            IConfiguration configuration,
            IHeaderContextService headerContextService)
        {
            _logger = logger;
            _context = context;
            _headerContextService = headerContextService;
        }

        public object GetSecurity()
        {
            var query = _context.SecurityAudit
                .AsNoTracking()
                .Include(x => x.UserId)
                .Select(x => new {
                    x.Id,
                    x.UserId,
                    x.User.Username,
                    x.IP,
                    x.LogTime,
                    x.Description
                });

            return query.ToList();
        }

        public object GetSecurityByUserId(Guid userId)
        {
            var query = _context.SecurityAudit
                .AsNoTracking()
                .Include(x => x.UserId)
                .Where(x => x.UserId == userId)
                .Select(x => new { 
                    x.Id,
                    x.UserId,
                    x.User.Username,
                    x.IP,
                    x.LogTime,
                    x.SecurityOperation,
                    x.Description
                });

            return query.ToList();
        }

        public object GetUserSessions(Guid userId)
        {
            var query = _context.SecurityAudit
                .AsNoTracking()
                .Include(x => x.UserId)
                .Where(x => x.UserId == userId && (
                    x.SecurityOperation == Enums.SecurityOperation.LOGIN_ATTEMPT_SUCCESS
                    || x.SecurityOperation == Enums.SecurityOperation.LOGIN_ATTEMPT_FAILS
                    || x.SecurityOperation == Enums.SecurityOperation.LOGOUT_ATTEMPT_SUCCESS
                    || x.SecurityOperation == Enums.SecurityOperation.LOGOUT_ATTEMPT_FAILS
                    ))
                .Select(x => new {
                    x.Id,
                    x.IP,
                    x.LogTime,
                    x.SecurityOperation,
                    x.Description
                });

            return query.ToList();
        }

        public object AuditDbTable(Guid userId, string dbTables, string tableId, DbOperations operation, string description)
        {

            DbTables.checkIsCorrect(dbTables);

            // FOR TEST ONLY - TODO::REMOVE it
            userId = _context.Users.AsNoTracking().Select(x => x.Id).First();

            var model = new Audit()
            {
                UserId = userId,
                DbTables = dbTables,
                TableRowId = tableId,
                Operation = operation,
                Time = DateTime.UtcNow,
                IP = _headerContextService.RemoteIpAddress(),
                Description = description
            };

            _context.Audits.Add(model);
            _context.SaveChanges();

            return model;
        }

        public object SecurityAudit(Guid? userId, SecurityOperation operation, string description)
        {

            var model = new SecurityAudit()
            {
                UserId = userId,
                SecurityOperation = operation,
                LogTime = DateTime.UtcNow,
                IP = _headerContextService.RemoteIpAddress(),
                Description = description + $" by: {null}"
            };

            _context.SecurityAudit.Add(model);
            _context.SaveChanges();

            return model;
        }

        public object SecurityAuditUserLoginAttemptSuccess(Guid userId, string username, string desc = "")
        {
            return SecurityAudit(userId, SecurityOperation.LOGIN_ATTEMPT_SUCCESS, $"{username}: Success login {desc}");
        }

        public object SecurityAuditUserLoginAttemptFails(Guid userId, string username, string desc = "")
        {
            return SecurityAudit(userId, SecurityOperation.LOGIN_ATTEMPT_FAILS, $"{username}: Login fails {desc}");
        }

        public object SecurityAuditUserLoginAttemptFails(string username, string desc = "")
        {
            return SecurityAudit(null, SecurityOperation.LOGIN_ATTEMPT_FAILS, $"{username}: Login fails {desc}");
        }

        public object SecurityAuditUserLogoutAttemptSuccess(Guid userId, string username, string desc = "")
        {
            return SecurityAudit(userId, SecurityOperation.LOGOUT_ATTEMPT_SUCCESS, $"{username}: Success logout {desc}");
        }

        public object SecurityAuditUserLogoutAttemptFails(Guid userId, string username, string desc = "")
        {
            return SecurityAudit(userId, SecurityOperation.LOGOUT_ATTEMPT_FAILS, $"{username}: Logout fails {desc}");
        }
    }
}