using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayerApp.Core.Entities;

namespace NLayerApp.Repository.Seeds
{
    public class ProductFeatureSeed : IEntityTypeConfiguration<ProductFeature>
    {
        private List<ProductFeature> _productFeatures = new()
        {
            new() {Id=1, Color="Red", Height=10, Weight=25, ProductId=1},
            new() {Id=2, Color="Blue", Height=20, Weight=50, ProductId=2},
            new() {Id=3, Color="Green", Height=30, Weight=40, ProductId=3},
        };

        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            builder.HasData(_productFeatures);
        }
    }
}
