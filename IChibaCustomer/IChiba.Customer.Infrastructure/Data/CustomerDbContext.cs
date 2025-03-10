using IChiba.Customer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IChiba.Customer.Infrastructure.Data;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
          : base(options)
    {
    }

    public DbSet<CustomerEntity> CustomerEntities { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=logistics_system;Username=postgres;Password=123123");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CustomerEntity>()
            .HasMany(s => s.Addresses)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


