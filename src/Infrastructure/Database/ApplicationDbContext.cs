using System;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Domain.Enums;

namespace Users.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100); 

            entity.Property(e => e.FullName)
                .IsRequired(false)
                .HasMaxLength(200);

            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.PasswordHash)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("now()");

            entity.Property(e => e.UpdatedAt)
                .IsRequired(false);

            entity.Property(e => e.Status).IsRequired()
                .HasDefaultValue(Statuses.Active);

            entity.Property(e => e.Role).IsRequired();
        });

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
