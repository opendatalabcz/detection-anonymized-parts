using Domain.DocumentAggregate;
using Domain.PageAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
    public class DappDbContext : DbContext
    {
        public DappDbContext(DbContextOptions<DappDbContext> options)
            : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; } = null!;

        public DbSet<Page> Pages { get; set; } = null!;

    }
}
