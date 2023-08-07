using Domain.DocumentAggregate;
using Domain.PageAggregate;
using Infrastructure.Configurations;
using Infrastructure.Persistance.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
    public class DappDbContext : DbContext
    {
        public string DbPath { get; }
        public string StoragePath { get; }
        public DappDbContext(DbContextOptions<DappDbContext> options)
            : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "DappDatabase.db");
            StoragePath = Path.Join(path, "DappStorage");
        }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Page> Pages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            modelBuilder.ApplyConfiguration(new PageConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }
}
