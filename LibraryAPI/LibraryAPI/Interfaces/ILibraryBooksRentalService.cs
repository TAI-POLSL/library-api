namespace LibraryAPI.Interfaces
{
    public interface ILibraryBooksRentalService
    {
        public object Get();
        public object GetById(int id);
        public object Add();
        public object Update(int id);
        public int End(int id);
    }
}
