namespace InventoryManager.Models.Entitites.Inventories {

    public class InventoryAccess {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public required string UserId { get; set; }
        public Inventory Inventory { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
