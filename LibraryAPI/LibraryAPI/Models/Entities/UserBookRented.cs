using LibraryAPI.Enums;

namespace LibraryAPI.Models.Entities
{
    public class UserBookRented
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int BookId { get; set; }
        public BookRentStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
    }
}
