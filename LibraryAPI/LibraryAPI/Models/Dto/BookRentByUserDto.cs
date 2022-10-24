using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.Dto
{
    public class BookRentByUserDto
    {
        public Guid UserId { get; set; }
        public int BookId { get; set; }
        public DateTime EndDate { get; set; }
    }
}
