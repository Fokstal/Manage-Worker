using ManageWorker_API.Models;
using Microsoft.EntityFrameworkCore;

namespace ManageWorker_API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Stuff> Stuff { get; set; }
        public DbSet<Worker> Worker { get; set; }
        public DbSet<User> User { get; set; }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=AppData/ManageWorkerDb.db");
        }
    }
}