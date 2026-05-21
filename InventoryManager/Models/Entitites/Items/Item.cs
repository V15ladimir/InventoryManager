using InventoryManager.Models.Entitites.Inventories;
using NpgsqlTypes;

namespace InventoryManager.Models.Entitites.Items {

    public class Item : BaseAudit {
        public int InventoryId { get; set; }
        public required string CustomId { get; set; }
        public required int Sequence { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public NpgsqlTsVector SearchVectorEn { get; set; } = null!;
        public NpgsqlTsVector SearchVectorRu { get; set; } = null!;
        public Inventory Inventory { get; set; } = null!;
        public ICollection<ItemValue> ItemValues { get; set; } = null!;
    }
}
