using NLayerApp.Core.DTOs;
using NLayerApp.Core.DTOs.ResponseDTOs;

namespace NLayerApp.Web.Services
{
    public class CategoryApiService
    {
        private readonly HttpClient _httpClient;

        public CategoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDto>?> GetAllAsync()
        {
            CustomResponseDto<List<CategoryDto>>? response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<CategoryDto>>>("categories");

            return response?.Data;
        }
    }
}
