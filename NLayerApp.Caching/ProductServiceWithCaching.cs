using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayerApp.Core.DTOs;
using NLayerApp.Core.DTOs.ResponseDTOs;
using NLayerApp.Core.Entities;
using NLayerApp.Core.Repositories;
using NLayerApp.Core.Services;
using NLayerApp.Core.UnitOfWorks;
using NLayerApp.Service.Exceptions;
using System.Linq.Expressions;

namespace NLayerApp.Caching
{
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "productsCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _uow;

        public ProductServiceWithCaching(IMapper mapper, IMemoryCache memoryCache, IProductRepository productRepository, IUnitOfWork uow)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _productRepository = productRepository;
            _uow = uow;

            if (!_memoryCache.TryGetValue(CacheProductKey, out _))
            {
                _memoryCache.Set(CacheProductKey, _productRepository.GetProductsWithCategoryAsync().Result);
            }

        }

        public async Task<CustomResponseDto<ProductDto>> AddAsync(CreateProductDto dto)
        {
            Product entity = _mapper.Map<Product>(dto);
            await _productRepository.AddAsync(entity);
            await _uow.CommitAsync();
            await CacheAllProductsAsync();

            return CustomResponseDto<ProductDto>.Success(StatusCodes.Status201Created, _mapper.Map<ProductDto>(entity));
        }

        public async Task<CustomResponseDto<ProductDto>> AddAsync(ProductDto dto)
        {
            Product entity = _mapper.Map<Product>(dto);
            await _productRepository.AddAsync(entity);
            await _uow.CommitAsync();
            await CacheAllProductsAsync();

            return CustomResponseDto<ProductDto>.Success(StatusCodes.Status201Created, _mapper.Map<ProductDto>(entity));
        }

        public async Task<CustomResponseDto<IEnumerable<ProductDto>>> AddRangeAsync(IEnumerable<CreateProductDto> dtos)
        {
            List<Product> entities = _mapper.Map<List<Product>>(dtos);
            await _productRepository.AddRangeAsync(entities);
            await _uow.CommitAsync();
            await CacheAllProductsAsync();

            return CustomResponseDto<IEnumerable<ProductDto>>.Success(StatusCodes.Status201Created, _mapper.Map<IEnumerable<ProductDto>>(entities));
        }

        public async Task<CustomResponseDto<IEnumerable<ProductDto>>> AddRangeAsync(IEnumerable<ProductDto> dtos)
        {
            List<Product> entities = _mapper.Map<List<Product>>(dtos);
            await _productRepository.AddRangeAsync(entities);
            await _uow.CommitAsync();
            await CacheAllProductsAsync();

            return CustomResponseDto<IEnumerable<ProductDto>>.Success(StatusCodes.Status201Created, _mapper.Map<IEnumerable<ProductDto>>(entities));
        }

        public async Task<CustomResponseDto<bool>> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponseDto<IEnumerable<ProductDto>>> GetAllAsync()
        {
            IEnumerable<Product> products = _memoryCache.Get<List<Product>>(CacheProductKey).ToList();

            return Task.FromResult(CustomResponseDto<IEnumerable<ProductDto>>.Success(StatusCodes.Status200OK, _mapper.Map<IEnumerable<ProductDto>>(products)));
        }

        public Task<CustomResponseDto<ProductDto>?> GetByIdAsync(int id)
        {
            // bizden task bekleniyor ancak cache den getirme async bir işlem değil.
            Product? product = _memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x => x.Id == id);

            return product == null
                ? throw new NotFoundException($"{typeof(Product).Name}({id}) not found")
                : Task.FromResult(CustomResponseDto<ProductDto>.Success(StatusCodes.Status200OK, _mapper.Map<ProductDto>(product)));
        }

        public Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategoryAsync()
        {
            List<Product> products = _memoryCache.Get<List<Product>>(CacheProductKey);
            List<ProductWithCategoryDto> productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);

            return Task.FromResult(CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsDto));
        }

        public async Task<CustomResponseDto<NoContentDto>> RemoveAsync(int id)
        {
            Product? entity = await _productRepository.GetByIdAsync(id);
            _productRepository.Remove(entity);
            await _uow.CommitAsync();
            await CacheAllProductsAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDto<NoContentDto>> RemoveRangeAsync(IEnumerable<int> ids)
        {
            List<Product> entities = await _productRepository.Where(x => ids.Contains(x.Id)).ToListAsync();
            _productRepository.RemoveRange(entities);
            await _uow.CommitAsync();
            await CacheAllProductsAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto)
        {
            _productRepository.Update(_mapper.Map<Product>(dto));
            await _uow.CommitAsync();
            await CacheAllProductsAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductDto dto)
        {
            _productRepository.Update(_mapper.Map<Product>(dto));
            await _uow.CommitAsync();
            await CacheAllProductsAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public Task<CustomResponseDto<IEnumerable<ProductDto>>> Where(Expression<Func<Product, bool>> expression)
        {
            List<Product> entities = _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).ToList();

            return Task.FromResult(CustomResponseDto<IEnumerable<ProductDto>>.Success(StatusCodes.Status200OK, _mapper.Map<IEnumerable<ProductDto>>(entities)));
        }


        private async Task CacheAllProductsAsync() => _memoryCache.Set(CacheProductKey, await _productRepository.GetProductsWithCategoryAsync());

    }
}
