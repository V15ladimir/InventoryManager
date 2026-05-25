namespace InventoryManager.Models.ViewModels.Inventories {

    public class InventoryCustomFieldsViewModel {
        public int InventoryId { get; set; }
        public List<InventoryFieldViewModel> CustomFields { get; set; } = [];
    }
}
