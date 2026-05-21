using InventoryManager.Models.Enums;
using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Models.ViewModels.Inventories.Index {

    public class InventoriesIndexViewModel {
        public ViewType ViewType { get; set; } = ViewType.All;
        public PagedList<InventoryViewModel> Inventories { get; set; } = new([]);
    }
}
