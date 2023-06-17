using AutoMapper;
using NLayerApp.Core.DTOs;
using NLayerApp.Core.DTOs.ResponseDTOs;

namespace NLayerApp.Web.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductWithCategoryDto>?> GetProductsWithCategoryAsync()
        {
            CustomResponseDto<List<ProductWithCategoryDto>>? response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<ProductWithCategoryDto>>>("Products/GetProductsWithCategory");

            return response?.Data;
        }

        public async Task<ProductDto?> SaveAsync(ProductDto newProduct)
        {
            var response = await _httpClient.PostAsJsonAsync("Products", newProduct);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            CustomResponseDto<ProductDto>? responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<ProductDto>>();

            return responseBody?.Data;
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            CustomResponseDto<ProductDto>? response = await _httpClient.GetFromJsonAsync<CustomResponseDto<ProductDto>>($"products/{id}");

            return response?.Data;
        }

        public async Task<bool> UpdateAsync(ProductDto newProduct)
        {
            var response = await _httpClient.PutAsJsonAsync("Products", newProduct);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Products/{id}");

            return response.IsSuccessStatusCode;
        }

    }
}
