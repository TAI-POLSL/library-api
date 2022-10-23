using LibraryAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.Models.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> modelBuilder) {
            modelBuilder.HasKey(s => s.Id);
            modelBuilder.Property(s => s.Id).HasDefaultValueSql("gen_random_uuid()").IsRequired();

            modelBuilder.Property(s => s.UserId).IsRequired();
            modelBuilder.Property(s => s.IpAddress).HasMaxLength(15).IsRequired();
            modelBuilder.Property(s => s.StartTime).IsRequired();

            modelBuilder.ToTable("SESSIONS");
            modelBuilder.Property(s => s.Id).HasColumnName("Id");
            modelBuilder.Property(s => s.UserId).HasColumnName("UserId");
            modelBuilder.Property(s => s.IpAddress).HasColumnName("IpAddress");
            modelBuilder.Property(s => s.StartTime).HasColumnName("StartTime");
        }
    }
}
