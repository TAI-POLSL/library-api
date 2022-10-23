using LibraryAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.Models.Configurations
{
    public class AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> modelBuilder)
        {
            modelBuilder.HasKey(p => p.Id);
            modelBuilder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(p => p.UserId).IsRequired();

            modelBuilder.Property(p => p.DbTables).HasMaxLength(25).IsRequired();
            modelBuilder.Property(p => p.TableRowId).IsRequired();
            modelBuilder.Property(p => p.Operation).IsRequired();
            modelBuilder.Property(p => p.Time).HasDefaultValue<DateTime>(DateTime.UtcNow).IsRequired();
            modelBuilder.Property(p => p.IP).HasMaxLength(15).IsRequired();
            modelBuilder.Property(p => p.Description).HasMaxLength(1000).IsRequired();

            modelBuilder.ToTable("Audits");
            modelBuilder.Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Property(p => p.UserId).HasColumnName("UserId");
            modelBuilder.Property(p => p.DbTables).HasColumnName("DbTables");
            modelBuilder.Property(p => p.TableRowId).HasColumnName("TableRowId");
            modelBuilder.Property(p => p.Operation).HasColumnName("Operation");
            modelBuilder.Property(p => p.IP).HasColumnName("IP");
            modelBuilder.Property(p => p.Time).HasColumnName("Time");
            modelBuilder.Property(p => p.Description).HasColumnName("Description");
        }
    }
}
