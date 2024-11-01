using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetPract2.Model;

namespace NetPract2
{
    class DBContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<DepositType> DepositTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=bank;Username=postgres;Password=postgres")
                .EnableSensitiveDataLogging().LogTo(Console.WriteLine,LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseIdentityByDefaultColumns();
        }
    }
}
