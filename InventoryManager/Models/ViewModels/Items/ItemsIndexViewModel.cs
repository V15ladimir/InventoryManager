using InventoryManager.Models.ViewModels.Inventories;

namespace InventoryManager.Models.ViewModels.Items {

    public class ItemsIndexViewModel {
        public int InventoryId { get; set; }
        public string InventoryName { get; set; } = string.Empty;
        public InventorySettingsViewModel Settings { get; set; } = new();
        public InventoryCustomIdPartsViewModel Parts { get; set; } = new();
        public InventoryCustomFieldsViewModel Fields { get; set; } = new();
        public bool HasSuperAccess { get; set; }
    }
}
