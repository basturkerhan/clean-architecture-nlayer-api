using NLayerApp.Core.DTOs;
using NLayerApp.Core.DTOs.ResponseDTOs;
using NLayerApp.Core.Entities;

namespace NLayerApp.Core.Services
{
    public interface IProductService : IService<Product, ProductDto>
    {
        Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategoryAsync();
        Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto);
        Task<CustomResponseDto<ProductDto>> AddAsync(CreateProductDto dto);
        Task<CustomResponseDto<IEnumerable<ProductDto>>> AddRangeAsync(IEnumerable<CreateProductDto> dtos);
    }
}
