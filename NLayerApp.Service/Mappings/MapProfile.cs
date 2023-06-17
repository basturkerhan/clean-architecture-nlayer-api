using AutoMapper;
using NLayerApp.Core.DTOs;
using NLayerApp.Core.Entities;

namespace NLayerApp.Service.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, ProductWithCategoryDto>();

            CreateMap<Category, CategoryWithProductsDto>();

        }
    }
}
