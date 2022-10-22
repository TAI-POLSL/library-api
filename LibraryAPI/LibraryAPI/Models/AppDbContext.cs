using LibraryAPI.Models.Entities;
using LibraryAPI.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<Person> Persons { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new UserCredentialConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
        #endregion
 
    }
}
