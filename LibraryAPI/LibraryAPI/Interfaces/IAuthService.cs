using LibraryAPI.Models.Dto;

namespace LibraryAPI.Interfaces
{
    public interface IAuthService
    {
        public object Login(LoginDto dto);
        public Task<object> Logout();
    }
}
