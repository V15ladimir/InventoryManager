namespace InventoryManager.Models.Dto {

    public class CreateInventoryDto {
        public string InventoryName { get; set; } = "New inventory";
        public int InventoryCategoryId { get; set; } = 1;
        public bool IsPublicInventory { get; set; }
        public string? InventoryDescription { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedById { get; set; }
    }
}
