using InventoryManager.Models.Enums;

namespace InventoryManager.Models.ViewModels.Inventories.Shared {

    public class InventoryFieldViewModel {
        public int Id { get; set; }
        public int Order { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public FieldType Type { get; set; }
        public FieldState State { get; set; } = FieldState.Optional;
    }
}
