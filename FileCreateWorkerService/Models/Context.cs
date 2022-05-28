using Microsoft.EntityFrameworkCore;

namespace FileCreateWorkerService.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
    }
}
