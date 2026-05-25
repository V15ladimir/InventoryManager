using InventoryManager.Models.Dto;
using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Services {

    public interface IItemService {
        Task<InventoryItemsDto> GetItemsAsync(int inventoryId, PagedRequest pagedRequest);
        Task<List<InventoryFieldDto>> GetItemFieldsAsync(int inventoryId);
        Task<List<InventoryItemValueDto>> GetItemValuesAsync(int itemId);
        Task<InventoryItemValuesDto> GetItemAsync(int itemId);
        Task<string> GetItemCustomIdAsync(int inventoryId);
        Task CreateItemAsync(CreateInventoryItemDto item);
        Task UpdateItemAsync(UpdateInventoryItemDto itemDto);
        Task DeleteItemsAsync(int inventoryId, List<int> selectedIds);
    }
}
