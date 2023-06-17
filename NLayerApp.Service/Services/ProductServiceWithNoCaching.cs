using AutoMapper;
using Microsoft.AspNetCore.Http;
using NLayerApp.Core.DTOs;
using NLayerApp.Core.DTOs.ResponseDTOs;
using NLayerApp.Core.Entities;
using NLayerApp.Core.Repositories;
using NLayerApp.Core.Services;
using NLayerApp.Core.UnitOfWorks;

namespace NLayerApp.Service.Services
{
    public class ProductServiceWithNoCaching : Service<Product, ProductDto>, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductServiceWithNoCaching(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IProductRepository productRepository, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
            _productRepository = productRepository;
        }

        public async Task<CustomResponseDto<ProductDto>> AddAsync(CreateProductDto dto)
        {
            Product entity = _mapper.Map<Product>(dto);
            await _productRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            // ef core id değerini entitye otomatik olarak atayacak
            return CustomResponseDto<ProductDto>.Success(StatusCodes.Status201Created, _mapper.Map<ProductDto>(entity));
        }

        public async Task<CustomResponseDto<IEnumerable<ProductDto>>> AddRangeAsync(IEnumerable<CreateProductDto> dtos)
        {
            List<Product> entities = _mapper.Map<List<Product>>(dtos);
            await _productRepository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<IEnumerable<ProductDto>>.Success(StatusCodes.Status201Created, _mapper.Map<IEnumerable<ProductDto>>(entities));
        }

        public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategoryAsync()
        {
            List<Product> products = await _productRepository.GetProductsWithCategoryAsync();
            List<ProductWithCategoryDto> productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);

            return CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsDto);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto)
        {
            _productRepository.Update(_mapper.Map<Product>(dto));
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }
    }
}
