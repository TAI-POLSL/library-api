using LibraryAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.Models.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> modelBuilder)
        {
            modelBuilder.HasKey(p => p.Id);
            modelBuilder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(p => p.AuthorFirstName).HasMaxLength(100).IsRequired();
            modelBuilder.Property(p => p.AuthorLastName).HasMaxLength(100).IsRequired();
            modelBuilder.Ignore(p => p.Author);
            modelBuilder.Property(p => p.Title).HasMaxLength(300).IsRequired();
            modelBuilder.Property(p => p.Description).HasMaxLength(1000).IsRequired();

            modelBuilder.ToTable("Books");
            modelBuilder.Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Property(p => p.AuthorFirstName).HasColumnName("AuthorFirstName");
            modelBuilder.Property(p => p.AuthorLastName).HasColumnName("AuthorLastName");
            modelBuilder.Property(p => p.Title).HasColumnName("Title");
            modelBuilder.Property(p => p.Description).HasColumnName("Description");
        }
    }
}
