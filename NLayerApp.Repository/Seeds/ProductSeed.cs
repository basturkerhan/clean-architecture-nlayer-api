using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayerApp.Core.Entities;

namespace NLayerApp.Repository.Seeds
{
    public class ProductSeed : IEntityTypeConfiguration<Product>
    {
        private List<Product> _products = new()
        {
            new() { Id=1, CategoryId=1, Name="Rotring Kalem", Price=100, Stock=50, CreatedDate=DateTime.Now},
            new() { Id=2, CategoryId=2, Name="Hasretinden Prangalar Eskittim", Price=50, Stock=10, CreatedDate=DateTime.Now},
            new() { Id=3, CategoryId=3, Name="Çizgili 60 Yapraklı Defter", Price=20, Stock=30, CreatedDate=DateTime.Now},
        };

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(_products);
        }
    }
}
