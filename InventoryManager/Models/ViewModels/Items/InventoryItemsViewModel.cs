using InventoryManager.Models.ViewModels.Inventories;
using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Models.ViewModels.Items {

    public class InventoryItemsViewModel {
        public int InventoryId { get; set; }
        public List<ItemFieldViewModel> Fields { get; set; } = [];
        public PagedList<ItemFieldValuesViewModel> Items = new([]);
        public bool HasSuperAccess { get; set; }
    }
}
