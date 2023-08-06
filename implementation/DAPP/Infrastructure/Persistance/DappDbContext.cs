using Domain.DocumentAggregate;
using Domain.PageAggregate;
using Infrastructure.Configurations;
using Infrastructure.Persistance.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistance
{
    public class DappDbContext : DbContext
    {
        public DappDbContext(DbContextOptions<DappDbContext> options)
            : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Page> Pages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            modelBuilder.ApplyConfiguration(new PageConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }

    public class DappDbContextFactory : IDesignTimeDbContextFactory<DappDbContext>
    {
        public DappDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DappDbContext>();
            optionsBuilder.UseSqlite("Data Source=dappDatabase.db");

            return new DappDbContext(optionsBuilder.Options);
        }
    }
}
