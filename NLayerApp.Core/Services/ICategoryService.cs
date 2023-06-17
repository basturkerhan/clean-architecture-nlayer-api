using NLayerApp.Core.DTOs;
using NLayerApp.Core.DTOs.ResponseDTOs;
using NLayerApp.Core.Entities;

namespace NLayerApp.Core.Services
{
    public interface ICategoryService : IService<Category, CategoryDto>
    {
        Task<CustomResponseDto<CategoryWithProductsDto?>> GetCategoryByIdWithProductsAsync(int id);
    }
}
