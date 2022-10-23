namespace LibraryAPI.Enums
{
    public static class DbTables
    {
        public static string BOOKS = "Books";
        public static string BOOKS_IN_LIBRARY = "BooksInLibrary";
        public static string PERSONS = "Persons";
        public static string USERS = "Users";
        public static string USERS_BOOKS_RENTED = "UserBookRented";
        public static string USERS_CREDENTIALS = "UsersCredentials";

        private static string[] ROM = new string[] { 
            BOOKS,
            BOOKS_IN_LIBRARY,
            PERSONS,
            USERS,
            USERS_BOOKS_RENTED,
            USERS_CREDENTIALS
        };

        public static string checkIsCorrect(string dbTable)
        {
            var test = ROM.Contains<string>(dbTable);
            if (test)
            {
                return dbTable;
            }

            throw new Exception("checkIsCorrect => invalid table name");
        }
    }
}
