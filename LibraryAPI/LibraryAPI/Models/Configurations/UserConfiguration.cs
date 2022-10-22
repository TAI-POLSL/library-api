﻿using LibraryAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.Models.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder.HasKey(u => u.Id);
            modelBuilder.Property(u => u.Id).HasDefaultValueSql("gen_random_uuid()").IsRequired();

            modelBuilder.Property(u => u.PersonId).IsRequired();
            modelBuilder.Property(u => u.UserCredentialId).IsRequired();

            modelBuilder.Property(u => u.Username).HasMaxLength(32).IsRequired();
            modelBuilder.Property(u => u.IsLocked).HasDefaultValue<bool>(false).IsRequired();
            modelBuilder.Property(u => u.IsConfirmed).HasDefaultValue<bool>(false).IsRequired();

            modelBuilder.HasIndex(p => p.Username).IsUnique(true);
            modelBuilder.HasIndex(p => p.PersonId).IsUnique(true);
            modelBuilder.HasIndex(p => p.UserCredentialId).IsUnique(true);

            modelBuilder.ToTable("Users");
            modelBuilder.Property(u => u.Id).HasColumnName("Id");
            modelBuilder.Property(u => u.Username).HasColumnName("Username");
            modelBuilder.Property(u => u.UserCredentialId).HasColumnName("UserCredentialId");
            modelBuilder.Property(u => u.PersonId).HasColumnName("PersonId");
            modelBuilder.Property(u => u.IsLocked).HasColumnName("IsLocked");
            modelBuilder.Property(u => u.IsConfirmed).HasColumnName("IsConfirmed");
        }
    }
}