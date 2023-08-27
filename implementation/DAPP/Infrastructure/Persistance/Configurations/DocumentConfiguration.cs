using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    /// <summary>
    /// Configuration for the documents table
    /// </summary>
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        /// <summary>
        /// Configures the documents table
        /// </summary>
        /// <param name="builder"> The builder</param>
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            ConfigureDocumentsTable(builder);
        }

        /// <summary>
        /// Configures the documents table
        /// </summary>
        /// <param name="builder"> The builder</param>
        private static void ConfigureDocumentsTable(EntityTypeBuilder<Document> builder)
        {
            // Table configuration
            builder.ToTable("Documents");

            // Key configuration
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value,
                value => DocumentId.Create(value))
                .IsRequired();


            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(d => d.Url)
                .IsRequired()
                .HasMaxLength(1024);

            builder.Property(d => d.Hash)
                .IsRequired()
                .HasMaxLength(255);

            // Ignore PageCount as it is a computed property
            builder.Ignore(d => d.PageCount);

            // Relationship configurations
            builder.HasMany(d => d.Pages)
                .WithOne(p => p.Document)
                .HasForeignKey(p => p.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
