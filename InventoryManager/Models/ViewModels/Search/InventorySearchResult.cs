namespace InventoryManager.Models.ViewModels.Search {

    public class InventorySearchResult {
        public int InventoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
