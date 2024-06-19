using Microsoft.EntityFrameworkCore;

namespace Middleware_REST_API.Model
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions<ContextDb> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
