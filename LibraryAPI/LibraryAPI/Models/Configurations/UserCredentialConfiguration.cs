using LibraryAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.Models.Configurations
{
    public class UserCredentialConfiguration : IEntityTypeConfiguration<UserCredential>
    {
        public void Configure(EntityTypeBuilder<UserCredential> modelBuilder)
        {
            modelBuilder.HasKey(u => u.Id);
            modelBuilder.Property(u => u.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(u => u.UserId).IsRequired();

            modelBuilder.Property(u => u.Password).IsRequired();
            modelBuilder.Property(u => u.ExpiredDate).IsRequired(false);
            modelBuilder.Property(u => u.ExpiredDate).HasDefaultValue<DateTime?>(null);
            modelBuilder.Property(u => u.IP).HasMaxLength(15).IsRequired();
            modelBuilder.Property(u => u.CreatedDate).HasDefaultValue<DateTime>(DateTime.UtcNow);

            modelBuilder.ToTable("UsersCredentials");
            modelBuilder.Property(u => u.Id).HasColumnName("Id");
            modelBuilder.Property(u => u.Password).HasColumnName("Password");
            modelBuilder.Property(u => u.UserId).HasColumnName("UserId");
            modelBuilder.Property(u => u.ExpiredDate).HasColumnName("ExpiredDate");
            modelBuilder.Property(u => u.IP).HasColumnName("IP");
            modelBuilder.Property(u => u.CreatedDate).HasColumnName("CreatedDate");
        }
    }
}
