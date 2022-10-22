namespace LibraryAPI.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public int UserCredentialId { get; set; }
        public Guid PersonId { get; set; }
        public bool IsLocked { get; set; }
        public bool IsConfirmed { get; set; }
        public virtual Person Person { get; set; }
        public virtual UserCredential UserCredential { get; set; }
    }
}
