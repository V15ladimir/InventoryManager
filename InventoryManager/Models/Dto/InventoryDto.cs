namespace InventoryManager.Models.Dto {

    public class InventoryDto {
        public int InventoryId { get; set; }
        public string InventoryName { get; set; } = string.Empty;
        public int InventoryCategoryId { get; set; } = 1;
        public bool IsPublicInventory { get; set; }
        public string? InventoryDescription { get; set; }
    }
}
