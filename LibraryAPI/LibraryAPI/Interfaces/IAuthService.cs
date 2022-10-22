namespace LibraryAPI.Interfaces
{
    public interface IAuthService
    {
        public string Login();
        public Task<string> Logout();
    }
}
