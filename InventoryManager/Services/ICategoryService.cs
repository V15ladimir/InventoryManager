using InventoryManager.Models.Dto;

namespace InventoryManager.Services {

    public interface ICategoryService {
        Task<List<InventoryCategoryDto>> GetCategoriesAsync();
    }
}
