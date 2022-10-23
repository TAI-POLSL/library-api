using LibraryAPI.Models.Entities;
using LibraryAPI.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookInLibrary> BooksInLibrary { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<SecurityAudit> SecurityAudit { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBookRented> UsersBooksRented { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) {}

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuditConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new BookInLibraryConfiguration());
            modelBuilder.ApplyConfiguration(new SecurityAuditConfiguration());
            modelBuilder.ApplyConfiguration(new SessionConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new UserBookRentedConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserCredentialConfiguration());
        }
        #endregion
 
    }
}
