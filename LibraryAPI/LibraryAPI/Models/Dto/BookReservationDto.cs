using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.Dto
{
    public class BookReservationDto
    {
        public Guid PersonId { get; set; }
        public int BookId { get; set; }
        public DateTime EndDate { get; set; }
    }
}
