namespace InventoryManager.Models.ViewModels.Inventories {

    public class InventoryCustomIdPartsViewModel {
        public int InventoryId { get; set; }
        public List<InventoryIdPartViewModel> CustomIdParts { get; set; } = [];
    }
}
