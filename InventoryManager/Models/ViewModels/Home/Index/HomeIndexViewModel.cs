using InventoryManager.Models.ViewModels.Inventories.Index;

namespace InventoryManager.Models.ViewModels.Home.Index {

    public class HomeIndexViewModel {
        public List<InventoryViewModel> LatestInventories { get; set; } = [];
        public List<InventoryViewModel> TopInventories { get; set; } = [];
    }
}
