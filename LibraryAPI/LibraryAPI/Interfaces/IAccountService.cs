using LibraryAPI.Models.Dto;

namespace LibraryAPI.Interfaces
{
    public interface IAccountService
    {
        public object Get(Guid? userId = null);
        public object GetAuditByUserId(Guid userId);
        public Task<object> Register(RegisterDto dto);
        public object Lock(Guid userId);
        public Task<object> ChangePassword(ChangePasswordDto dto, Guid userId);
        public object Close(Guid userId);
        public Task<object> GenerateAdmin();
        
    }
}
