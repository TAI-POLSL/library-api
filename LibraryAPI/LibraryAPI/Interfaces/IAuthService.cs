using LibraryAPI.Models.Dto;

namespace LibraryAPI.Interfaces
{
    public interface IAuthService
    {
        public Task<object> LoginAsync(LoginDto dto);
        public Task<object> Logout();
    }
}
