using LibraryAPI.Models.Dto;

namespace LibraryAPI.Interfaces
{
    public interface IAccountService
    {
        public Task<object> Register(RegisterDto dto);
        public object Lock(Guid userId);
        public object ChangePassword(ChangePasswordDto dto);
        public object Close(Guid userId);
    }
}
