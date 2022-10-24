using LibraryAPI.Models.Dto;

namespace LibraryAPI.Interfaces
{
    public interface IAccountService
    {
        public object GetStrategy(Guid? userId = null);
        public Task<object> Register(RegisterDto dto);
        public object ChangeAccountLockStatus(Guid userId, bool status = true);
        public Task<object> ChangePassword(ChangePasswordDto dto);
        public object CloseStrategy(Guid? userId);
        public Task<object> GenerateAdmin();
        public object? GetOwn();
    }
}
