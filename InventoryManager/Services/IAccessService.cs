namespace InventoryManager.Services {

    public interface IAccessService {
        Task<bool> CanEditInventoryAsync(int inventoryId, string? userId);
        Task<bool> CanEditItemsAsync(int inventoryId, string? userId);
    }
}
