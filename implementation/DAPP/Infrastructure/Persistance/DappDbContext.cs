using Domain.DocumentAggregate;
using Domain.PageAggregate;
using Infrastructure.Configurations;
using Infrastructure.Persistance.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
    /// <summary>
    /// The database context for the application
    /// </summary>
    public class DappDbContext : DbContext
    {
        /// <summary>
        /// The path to the database
        /// </summary>
        public string DbPath { get; }
        /// <summary>
        /// The path to the storage
        /// </summary>
        public string StoragePath { get; }
        /// <summary>
        /// The documents in the database
        /// </summary>
        public DbSet<Document> Documents { get; set; }

        /// <summary>
        /// The pages in the database
        /// </summary>
        public DbSet<Page> Pages { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public DappDbContext(DbContextOptions<DappDbContext> options)
            : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "DappDatabase.db");
            StoragePath = Path.Join(path, "DappStorage");
        }

        /// <summary>
        /// On model creating
        /// </summary>
        /// <param name="modelBuilder"> The model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            modelBuilder.ApplyConfiguration(new PageConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        /// <summary>
        /// On configuring
        /// </summary>
        /// <param name="options"> The options</param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseLazyLoadingProxies();
            options.UseSqlite($"Data Source={DbPath}");
        }
    }
}
