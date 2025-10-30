using System.Reflection;
using Contax.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contax.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Username).IsRequired().HasMaxLength(256);
            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(512);
            builder.Property(u => u.Role).IsRequired().HasMaxLength(50);

            builder.HasIndex(u => u.Username).IsUnique();
            builder.ToTable("Users");
        });

        string adminPasswordHash = "$2a$12$wEkPcsAcc2fFelvm560eF.5cdj.OxU8HDWWVa/xxM0oStkbiESZZK";

        modelBuilder
            .Entity<User>()
            .HasData(
                new
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Username = "ewerthon32santana@gmail.com",
                    PasswordHash = adminPasswordHash,
                    Role = "Admin",
                }
            );
        base.OnModelCreating(modelBuilder);
    }
}
