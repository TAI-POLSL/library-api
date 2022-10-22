using LibraryAPI.Enums;

namespace LibraryAPI.Models.Entities
{
    public class SecurityAudit
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public string IP { get ; set; }
        public SecurityOperation SecurityOperation { get; set;}
        public string Description { get; set; }
        public DateTime LogTime { get; set; }
        public User User { get; set; } 
    }
}
