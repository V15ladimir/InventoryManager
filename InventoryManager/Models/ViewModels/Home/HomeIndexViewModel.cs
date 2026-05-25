using InventoryManager.Models.ViewModels.Inventories;

namespace InventoryManager.Models.ViewModels.Home {

    public class HomeIndexViewModel {
        public List<InventoryViewModel> LatestInventories { get; set; } = [];
        public List<InventoryViewModel> TopInventories { get; set; } = [];
    }
}
