using InventoryManager.Models.Enums;

namespace InventoryManager.Models.ViewModels.Inventories.Shared {

    public class ItemFieldViewModel {
        public int Id { get; set; }
        public FieldType Type { get; set; }
        public string? Name { get; set; }
    }
}
