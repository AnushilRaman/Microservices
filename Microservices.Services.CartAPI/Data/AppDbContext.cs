using Microservices.Services.CartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.CartAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<CartHeaders> CartHeaders { get; set; }
    }
}
