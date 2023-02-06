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

        public object GetAudits()
        {
            var query = _context.Audits
                .AsNoTracking()
                .Include(x => x.User)
                .Select(x => new {
                    x.Id,
                    x.UserId,
                    x.User.Username,
                    x.Operation,
                    x.IP,
                    x.Time,
                    x.Description,
                    x.TableRowId,
                    x.DbTables
                });

            return query.ToList();
        }

        public object GetSecurity()
        {
            var query = _context.SecurityAudit
                .AsNoTracking()
                .Include(x => x.User)
                .Select(x => new {
                    x.Id,
                    x.UserId,
                    x.User.Username,
                    x.IP,
                    x.LogTime,
                    x.Description,
                    x.OperatorUserRole,
                    x.OperatorUserUsername
                });

            return query.ToList();
        }

        public object GetSecurityByUserId(Guid userId)
        {
            var query = _context.SecurityAudit
                .AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.UserId == userId)
                .Select(x => new { 
                    x.Id,
                    x.UserId,
                    x.User.Username,
                    x.IP,
                    x.LogTime,
                    x.SecurityOperation,
                    x.Description,
                    x.OperatorUserRole,
                    x.OperatorUserUsername
                });

            return query.ToList();
        }

        public object GetUserSessions(Guid userId)
        {
            var query = _context.Sessions
                .AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.UserId == userId)
                .Select(x => new {
                    x.Id,
                    x.UserId,
                    x.IpAddress,
                    x.StartTime,
                });

            return query.ToList();
        }

        public object AuditDbTable(string dbTables, string tableId, DbOperations operation, string description)
        {
            DbTables.checkIsCorrect(dbTables);

            var model = new Audit()
            {
                UserId = _headerContextService.GetUserId(),
                DbTables = dbTables,
                TableRowId = tableId,
                Operation = operation,
                Time = DateTime.UtcNow,
                IP = _headerContextService.GetUserRemoteIpAddress(),
                Description = description
            };

            _context.Audits.Add(model);
            _context.SaveChanges();

            return model;
        }

        public object SecurityAudit(Guid? userId, SecurityOperation operation, string description)
        {
            Guid? OperatorUserId = null;
            string? OperatorUserUsername = null;
            UserRoles OperatorUserRole = UserRoles.GUEST;
            
            if (_headerContextService.IsAuthenticated() == true)
            {
                OperatorUserId = _headerContextService.GetUserId();
                OperatorUserUsername = _headerContextService.GetUserUsername();
                OperatorUserRole = _headerContextService.GetUserRole();
            }

            var model = new SecurityAudit()
            {
                UserId = userId,
                SecurityOperation = operation,
                LogTime = DateTime.UtcNow,
                IP = _headerContextService.GetUserRemoteIpAddress(),
                Description = description,
                OperatorUserId = OperatorUserId,
                OperatorUserUsername = OperatorUserUsername,
                OperatorUserRole = OperatorUserRole,
            };

            _context.SecurityAudit.Add(model);
            _context.SaveChanges();

            return model;
        }

        public object SecurityAuditUserLoginAttemptSuccess(Guid userId, string username, string desc = "")
        {
            desc = desc == "" ? $"{username}: Success login" : $"{username}: Success login {desc}";
            return SecurityAudit(userId, SecurityOperation.LOGIN_ATTEMPT_SUCCESS, desc);
        }

        public object SecurityAuditUserLoginAttemptFails(Guid? userId, string username, string desc = "")
        {
            desc = desc == "" ? $"{username}: Login fails" : $"{username}: Login fails {desc}";
            return SecurityAudit(userId, SecurityOperation.LOGIN_ATTEMPT_FAILS, desc);
        }

        public object SecurityAuditUserLogoutAttemptSuccess(Guid userId, string username, string desc = "")
        {
            desc = desc == "" ? $"{username}: Success logout" : $"{username}: Success logout {desc}";
            return SecurityAudit(userId, SecurityOperation.LOGOUT_ATTEMPT_SUCCESS, desc);
        }

        public object SecurityAuditUserLogoutAttemptFails(Guid userId, string username, string desc = "")
        {
            desc = desc == "" ? $"{username}: Logout fails" : $"{username}: Logout fails {desc}";
            return SecurityAudit(userId, SecurityOperation.LOGOUT_ATTEMPT_FAILS, desc);
        }
    }
}