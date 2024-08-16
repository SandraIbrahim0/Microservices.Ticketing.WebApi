using Ticketing.Microservice.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Microservice.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<Ticket> Tickets { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>().HasKey(p => p.Id);
        modelBuilder.Entity<Ticket>().HasData(
            new Ticket("Ali", "London", "Ali@gmail.com", 1500),
            new Ticket("Samy", "Egypt", "Samy@gmail.com", 1500),
            new Ticket("Amira", "Dubai", "Amira@gmail.com", 1500)
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("ticketingSystemDB");
    }
}
