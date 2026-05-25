using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Models.ViewModels.Inventories {

    public class InventoryAccessListViewModel {
        public int InventoryId { get; set; }
        public PagedList<InventoryAccessViewModel> AccessList { get; set; } = new PagedList<InventoryAccessViewModel>([]);
    }

    public class InventoryAccessViewModel {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool HasAccess { get; set; }
    }
}
