using InventoryManager.Models.Enums;

namespace InventoryManager.Models.Dto {

    public class InventoryFieldDto {
        public int Id { get; set; }
        public int Order { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public FieldType FieldType { get; set; }
        public FieldState FieldState { get; set; }
    }
}
