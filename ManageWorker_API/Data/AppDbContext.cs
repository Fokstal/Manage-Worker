using ManageWorker_API.Models;
using ManageWorker_API.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace ManageWorker_API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<StuffDTO> Stuff { get; set; }

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