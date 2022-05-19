using Microsoft.EntityFrameworkCore;

namespace WatermarkWebApp.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
