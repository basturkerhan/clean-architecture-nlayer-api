using AutoMapper;
using NLayerApp.Core.CustomMessages;
using NLayerApp.Core.DTOs;
using NLayerApp.Core.DTOs.ResponseDTOs;
using NLayerApp.Core.Entities;
using NLayerApp.Core.Repositories;
using NLayerApp.Core.Services;
using NLayerApp.Core.UnitOfWorks;

namespace NLayerApp.Service.Services
{
    public class CategoryService : Service<Category, CategoryDto>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitOfWork, IMapper mapper, ICategoryRepository categoryRepository) : base(repository, unitOfWork, mapper)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CustomResponseDto<CategoryWithProductsDto?>> GetCategoryByIdWithProductsAsync(int id)
        {
            Category? category = await _categoryRepository.GetCategoryByIdWithProductsAsync(id);
            if (category == null)
            {
                return CustomResponseDto<CategoryWithProductsDto?>.Fail(404, ErrorMessages.NotFound("category"));
            }

            CategoryWithProductsDto dto = _mapper.Map<CategoryWithProductsDto>(category);

            return CustomResponseDto<CategoryWithProductsDto?>.Success(200, dto);
        }
    }
}
