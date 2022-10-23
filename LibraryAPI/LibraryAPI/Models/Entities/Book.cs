namespace LibraryAPI.Models.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string Author => $"{AuthorFirstName} {AuthorLastName}";
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual BookInLibrary BookInLibrary { get; set; } = new BookInLibrary();
        public virtual ICollection<UserBookRented> UserBookRented { get; set; } = new HashSet<UserBookRented>();
    }
}
