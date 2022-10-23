using LibraryAPI.Models.Dto;

namespace LibraryAPI.Interfaces
{
    public interface IAuditService
    {
        public object GetSecurity();
        public object GetSecurityByUserId(Guid userId);
        public object GetUserSessions(Guid userId);
    }
}
