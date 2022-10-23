using LibraryAPI.Models.Dto;

namespace LibraryAPI.Interfaces
{
    public interface ILibraryBooksService
    {
        public object Get(int? id = null);
        public object Add(BookDto dto);
        public object Update(int id, BookDto dto);
        public object UpdateTotalQuantity(int id, int quantity);
        public int Remove(int id);
    }
}
