using LibraryAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.Models.Configurations
{
    public class BookInLibraryConfiguration : IEntityTypeConfiguration<BookInLibrary>
    {
        public void Configure(EntityTypeBuilder<BookInLibrary> modelBuilder)
        {
            modelBuilder.HasKey(p => p.Id);
            modelBuilder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(p => p.BookId).IsRequired();

            modelBuilder.Property(p => p.NumOfAvailable).HasDefaultValue<int>(0).IsRequired();
            modelBuilder.Property(p => p.NumOfRented).HasDefaultValue<int>(0).IsRequired();
            modelBuilder.Property(p => p.TotalBooks).HasDefaultValue<int>(0).IsRequired();

            modelBuilder.ToTable("BooksInLibrary");
            modelBuilder.Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Property(p => p.BookId).HasColumnName("BookId");
            modelBuilder.Property(p => p.NumOfAvailable).HasColumnName("NumOfAvailable");
            modelBuilder.Property(p => p.NumOfRented).HasColumnName("NumOfRented");
            modelBuilder.Property(p => p.TotalBooks).HasColumnName("TotalBooks");
        }
    }
}
