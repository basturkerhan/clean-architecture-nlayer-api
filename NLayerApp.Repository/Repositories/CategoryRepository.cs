using Microsoft.EntityFrameworkCore;
using NLayerApp.Core.Entities;
using NLayerApp.Core.Repositories;

namespace NLayerApp.Repository.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetCategoryByIdWithProductsAsync(int id)
        {
            // Single ve First farkı ne? birden fazla eşleşen kayıt olursa Single hata döner, First ise ilkini döner
            return await _context.Set<Category>().Include(x => x.Products).Where(x => x.Id == id).SingleOrDefaultAsync();
        }

    }
}
