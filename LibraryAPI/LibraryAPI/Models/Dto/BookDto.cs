using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.Dto
{
    public class BookDto
    {
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TotalBooks { get; set; }
    }
}
