using LibraryAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.Models.Configurations
{
    public class SecurityAuditConfiguration : IEntityTypeConfiguration<SecurityAudit>
    {
        public void Configure(EntityTypeBuilder<SecurityAudit> modelBuilder)
        {
            modelBuilder.HasKey(p => p.Id);
            modelBuilder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(p => p.UserId).IsRequired(false);

            modelBuilder.Property(p => p.IP).HasMaxLength(15).IsRequired();
            modelBuilder.Property(p => p.SecurityOperation).IsRequired();
            modelBuilder.Property(p => p.Description).HasMaxLength(1000).IsRequired();
            modelBuilder.Property(p => p.LogTime).HasDefaultValue<DateTime>(DateTime.UtcNow).IsRequired();

            modelBuilder.Property(p => p.OperatorUserId).IsRequired(false);
            modelBuilder.Property(p => p.OperatorUserUsername).HasMaxLength(32).IsRequired(false);
            modelBuilder.Property(p => p.OperatorUserRole).HasConversion<string>().IsRequired();


            modelBuilder.ToTable("SecurityAudit");
            modelBuilder.Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Property(p => p.UserId).HasColumnName("UserId");
            modelBuilder.Property(p => p.IP).HasColumnName("IP");
            modelBuilder.Property(p => p.SecurityOperation).HasColumnName("SecurityOperation");
            modelBuilder.Property(p => p.Description).HasColumnName("Description");
            modelBuilder.Property(p => p.LogTime).HasColumnName("LogTime");
            modelBuilder.Property(p => p.Description).HasColumnName("Description");
            modelBuilder.Property(p => p.OperatorUserId).HasColumnName("OperatorUserId");
            modelBuilder.Property(p => p.OperatorUserUsername).HasColumnName("OperatorUserUsername");
            modelBuilder.Property(p => p.OperatorUserRole).HasColumnName("OperatorUserRole");
        }
    }
}
