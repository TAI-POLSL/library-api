using LibraryAPI.Enums;
using LibraryAPI.Models.Dto;

namespace LibraryAPI.Interfaces
{
    public interface IAuditService
    {
        public object GetAudits();
        public object GetSecurity();
        public object GetSecurityByUserId(Guid userId);
        public object GetUserSessions(Guid userId);
        public object SecurityAuditUserLoginAttemptSuccess(Guid userId, string username, string desc = "");
        public object SecurityAuditUserLoginAttemptFails(Guid? userId, string username, string desc = "");
        public object SecurityAuditUserLogoutAttemptSuccess(Guid userId, string username, string desc = "");
        public object SecurityAuditUserLogoutAttemptFails(Guid userId, string username, string desc = "");
        public object SecurityAudit(Guid? userId, SecurityOperation operation, string description);
        public object AuditDbTable(string dbTables, string tableId, DbOperations operation, string description);
    }
}
