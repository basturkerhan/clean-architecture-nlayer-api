using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayerApp.Core.Entities;

namespace NLayerApp.Repository.Configurations
{
    public class ProductFeatureConfiguration : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            builder.Property(pf => pf.Color).IsRequired().HasMaxLength(10);
            builder.Property(pf => pf.Height).IsRequired();
            builder.Property(pf => pf.Weight).IsRequired();
            builder
                .HasOne(pf => pf.Product)
                .WithOne(p => p.ProductFeature)
                .HasForeignKey<ProductFeature>(pf => pf.ProductId);
        }
    }
}
