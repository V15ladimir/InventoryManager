using InventoryManager.Models.ViewModels.Inventories.Form;

namespace InventoryManager.Models.ViewModels.Inventories.Shared {

    public class InventoryCustomIdPartsViewModel {
        public int InventoryId { get; set; }
        public List<InventoryIdPartViewModel> CustomIdParts { get; set; } = [];
    }
}
