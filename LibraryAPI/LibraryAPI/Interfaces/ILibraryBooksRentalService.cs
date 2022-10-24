using LibraryAPI.Models.Dto;

namespace LibraryAPI.Interfaces
{
    public interface ILibraryBooksRentalService
    {
        public object Get(int? id = null, Guid? userId = null);
        public object Add(BookRentByUserDto dto);
        public object Cancel(int id);
        public object End(int id);
    }
}
