namespace InventoryManager.Models.Entitites.Inventories {

    public class Category {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Inventory> Inventories { get; set; } = [];
    }
}
