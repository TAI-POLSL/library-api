namespace LibraryAPI.Models.Entities
{
    public class BookInLibrary
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int NumOfAvailable { get; set; }
        public int NumOfRented { get; set; }
        public int TotalBooks { get; set; }
        public virtual Book Book { get; set; }
    }
}
