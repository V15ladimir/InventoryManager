using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Models.Entitites.Items;
using InventoryManager.Models.Enums;

namespace InventoryManager.Models.Entitites.Custom {

    public abstract class Field {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }
        public FieldState FieldState { get; set; }
        public FieldType FieldType { get; set; }
        public Inventory Inventory { get; set; } = null!;
        public ICollection<ItemValue> ItemValues { get; set; } = [];
    }

    public class SinglelineField : Field {
        public int? Length { get; set; }
        public string? Regex { get; set; }
    }

    public class MultilineField : Field {
        public int? Length { get; set; }
        public string? Regex { get; set; }
    }

    public class NumberField : Field {
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }

    public class LinkField : Field { }

    public class BooleanField : Field { }
}
