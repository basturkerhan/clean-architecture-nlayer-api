using NLayerApp.Core.Entities;

namespace NLayerApp.Core.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetCategoryByIdWithProductsAsync(int id);
    }
}
