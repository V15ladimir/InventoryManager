using InventoryManager.Models.Entitites.Custom;

namespace InventoryManager.Models.Entitites.Items {

    public class ItemValue {
        public int Id {  get; set; }
        public int ItemId { get; set; }
        public int FieldId { get; set; }
        public string? Value { get; set; }
        public Field Field { get; set; } = null!;
        public Item Item { get; set; } = null!;
    }
}
