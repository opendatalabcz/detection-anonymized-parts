using Domain.PageAggregate;
using Domain.PageAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations
{
    public class PageConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            ConfigurePagesTable(builder);
        }

        private static void ConfigurePagesTable(EntityTypeBuilder<Page> builder)
        {
            // Table configuration
            builder.ToTable("Pages");

            // Key configuration
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value,
                value => PageId.Create(value))
                .IsRequired();



            builder.Property(p => p.OriginalImageUrl)
                .HasMaxLength(1024);

            builder.Property(p => p.ResultImageUrl)
                .HasMaxLength(1024);

            builder.Property(p => p.AnonymizationResult);

            // Relationship configurations
            builder.HasOne(p => p.Document)
               .WithMany(d => d.Pages)
               .HasForeignKey(p => p.DocumentId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
