using LibraryAPI.Enums;

namespace LibraryAPI.Models.Entities
{
    public class Audit
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string DbTables { get; set; }
        public string TableRowId { get; set; }
        public DbOperations Operation { get; set; } 
        public DateTime Time { get; set; }
        public string IP { get; set; }
        public string Description { get; set; }
        public virtual User User { get; set; }
    }
}
