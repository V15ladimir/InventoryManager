using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Models.ViewModels.Inventories.Index;
using InventoryManager.Models.ViewModels.Inventories.Shared;
using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Services {

    public interface IInventoryService {
        Task<InventoryDto> GetInventoryAsync(int inventoryId);
        Task<List<InventoryViewModel>> GetLatestInventories();
        Task<List<InventoryViewModel>> GetTopInventories();
        Task<PagedList<InventoryViewModel>> GetInventoriesAsync(PagedRequest pagedRequest);
        Task<PagedList<InventoryViewModel>> GetMyInventoriesAsync(string? userId, PagedRequest pagedRequest);
        Task<PagedList<InventoryViewModel>> GetSharedInventoriesAsync(string? userId, PagedRequest pagedRequest);
        Task<PagedList<InventoryAccessDto>> GetInventoryAccessAsync(int inventoryId, PagedRequest pagedRequest);
        Task<Inventory> CreateInventoryAsync(CreateInventoryDto inventoryDto);
        Task<Inventory> UpdateInventoryAsync(UpdateInventoryDto inventoryDto);
        Task UpdateCustomIdPartsAsync(int inventoryId, InventoryCustomIdPartsViewModel parts);
        Task UpdateCustomFieldsAsync(int inventoryId, InventoryCustomFieldsViewModel fields);
        Task UpdateInventoryAccessAsync(UpdateInventoryAccessDto access);
        Task<int> DeleteInventoryAsync(List<int> inventoryIds);
        Task<List<InventoryIdPartDto>> GetInventoryIdPartsAsync(int inventoryId);
        Task<List<InventoryFieldDto>> GetInventoryFieldsAsync(int inventoryId);
    }
}
