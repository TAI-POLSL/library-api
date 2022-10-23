using LibraryAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.Models.Configurations
{
    public class UserBookRentedConfiguration : IEntityTypeConfiguration<UserBookRented>
    {
        public void Configure(EntityTypeBuilder<UserBookRented> modelBuilder)
        {
            modelBuilder.HasKey(p => p.Id);
            modelBuilder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(p => p.UserId).IsRequired();
            modelBuilder.Property(p => p.BookId).IsRequired();

            modelBuilder.Property(p => p.Status).IsRequired();
            modelBuilder.Property(p => p.StartDate).HasDefaultValue<DateTime>(DateTime.UtcNow).IsRequired();
            modelBuilder.Property(p => p.EndDate).IsRequired();


            modelBuilder.ToTable("UsersBooksRented");
            modelBuilder.Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Property(p => p.UserId).HasColumnName("UserId");
            modelBuilder.Property(p => p.BookId).HasColumnName("BookId");
            modelBuilder.Property(p => p.Status).HasColumnName("Status");
            modelBuilder.Property(p => p.StartDate).HasColumnName("StartDate");
            modelBuilder.Property(p => p.EndDate).HasColumnName("EndDate");
        }
    }
}
