namespace LibraryAPI.Interfaces
{
    public interface ILibraryBooksService
    {
        public object Get();
        public object GetById(int id);
        public object Add();
        public object Update(int id);
        public object UpdateTotalQuantity(int id);
        public int Remove(int id);
    }
}
