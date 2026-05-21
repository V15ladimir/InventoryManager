namespace InventoryManager.Models.ViewModels.Inventories.Shared {

    public class InventoryCustomFieldsViewModel {
        public int InventoryId { get; set; }
        public List<InventoryFieldViewModel> CustomFields { get; set; } = [];
    }
}
