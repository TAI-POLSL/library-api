using LibraryAPI.Enums;

namespace LibraryAPI.Models.Entities
{
    public class Session
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string IpAddress { get; set; }
        public DateTimeOffset StartTime { get; set; } = DateTimeOffset.UtcNow;
        public virtual User User { get; set; }  
    }
}
