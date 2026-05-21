using InventoryManager.Models.Entitites.Custom;
using InventoryManager.Models.Entitites.Items;
using NpgsqlTypes;

namespace InventoryManager.Models.Entitites.Inventories {

    public class Inventory : BaseAudit {
        public required string Name { get; set; }
        public int CategoryId { get; set; }
        public bool IsPublic { get; set; }
        public string? Description { get; set; }
        public NpgsqlTsVector SearchVectorEn { get; set; } = null!;
        public NpgsqlTsVector SearchVectorRu { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public ICollection<Field> Fields { get; set; } = [];
        public ICollection<IdPart> Elements { get; set; } = [];
        public ICollection<Item> Items { get; set; } = [];
        public ICollection<InventoryAccess> AccessList { get; set; } = [];
        public ICollection<InventoryDiscussion> Discussions { get; set; } = [];
    }
}
