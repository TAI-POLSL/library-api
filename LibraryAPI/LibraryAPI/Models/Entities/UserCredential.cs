namespace LibraryAPI.Models.Entities
{
    public class UserCredential
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public Guid UserId { get; set; }
        public string IP { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public virtual User User { get; set; } = new User();
    }
}
