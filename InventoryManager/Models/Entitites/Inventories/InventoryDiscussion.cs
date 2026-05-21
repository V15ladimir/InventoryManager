namespace InventoryManager.Models.Entitites.Inventories {

    public class InventoryDiscussion {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public required string Content { get; set; }
        public required string CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public Inventory Inventory { get; set; } = null!;
        public ApplicationUser CreatedBy { get; set; } = null!;
    }
}
