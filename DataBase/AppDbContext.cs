using Microsoft.EntityFrameworkCore;
using PriceService.Model;

namespace PriceService.DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt): base(opt)
        {
            
        }

        public DbSet<Price> Prices { get; set; }
        public DbSet<Platform> Platforms { get; set; }
    }
}