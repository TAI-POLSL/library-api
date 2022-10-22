namespace LibraryAPI.Interfaces
{
    public interface IAccountService
    {
        public object Register();
        public object Lock(Guid userId);
        public object ChangePassword();
        public object Close();
    }
}
