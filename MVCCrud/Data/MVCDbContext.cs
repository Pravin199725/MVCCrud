using Microsoft.EntityFrameworkCore;
using MVCCrud.Models.Domain;

namespace MVCCrud.Data
{
    public class MVCDbContext : DbContext
    {
        public MVCDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
