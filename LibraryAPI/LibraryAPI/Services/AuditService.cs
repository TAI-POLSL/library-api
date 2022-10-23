using LibraryAPI.Interfaces;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services
{
    public class AuditService : IAuditService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly AppDbContext _context;

        public AuditService(
            ILogger<AuthService> logger,
            AppDbContext context,
            IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
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
                .Where(x => x.UserId == userId && (x.SecurityOperation == Enums.SecurityOperation.LOGIN || x.SecurityOperation == Enums.SecurityOperation.LOGOUT))
                .Select(x => new {
                    x.Id,
                    x.IP,
                    x.LogTime,
                    x.SecurityOperation,
                    x.Description
                });

            return query.ToList();
        }
    }
}