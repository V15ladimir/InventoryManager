using InventoryManager.Models.ViewModels.Inventories.Form;
using InventoryManager.Models.ViewModels.Inventories.Shared;

namespace InventoryManager.Models.ViewModels.Items.Index {

    public class ItemsIndexViewModel {
        public int InventoryId { get; set; }
        public InventorySettingsViewModel Settings { get; set; } = new();
        public InventoryCustomIdPartsViewModel Parts { get; set; } = new();
        public InventoryCustomFieldsViewModel Fields { get; set; } = new();
        public bool HasSuperAccess { get; set; }
    }
}
