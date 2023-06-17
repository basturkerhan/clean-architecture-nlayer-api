using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayerApp.Core.Entities;

namespace NLayerApp.Repository.Seeds
{
    public class CategorySeed : IEntityTypeConfiguration<Category>
    {
        private List<Category> _categories = new() {
            new() {Id=1, Name="Kalemler", CreatedDate=DateTime.Now},
            new() {Id=2, Name="Kitaplar", CreatedDate=DateTime.Now},
            new() {Id=3, Name="Defterler", CreatedDate=DateTime.Now},
        };

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(_categories);
        }
    }
}
