using InventoryManager.Models.ViewModels.Inventories.Shared;
using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Models.ViewModels.Items.Index {

    public class InventoryItemsViewModel {
        public int InventoryId { get; set; }
        public List<ItemFieldViewModel> Fields { get; set; } = [];
        public PagedList<ItemFieldValuesViewModel> Items = new([]);
    }
}
