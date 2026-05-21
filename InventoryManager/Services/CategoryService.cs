using InventoryManager.Data;
using InventoryManager.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Services {

    public class CategoryService(ApplicationDbContext context) : ICategoryService {

        public async Task<List<InventoryCategoryDto>> GetCategoriesAsync() {
            return await context.Categories.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new InventoryCategoryDto { 
                    CategoryId = x.Id, 
                    CategoryName = x.Name
                })
                .ToListAsync();
        }
    }
}
