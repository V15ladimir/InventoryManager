namespace InventoryManager.Models.Dto {

    public class UpdateInventoryDto {
        public int InventoryId { get; set; }
        public string InventoryName { get; set; } = string.Empty;
        public int InventoryCategoryId { get; set; }
        public bool IsPublicInventory { get; set; }
        public string? InventoryDescription { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? UpdatedById { get; set; }
    }
}
