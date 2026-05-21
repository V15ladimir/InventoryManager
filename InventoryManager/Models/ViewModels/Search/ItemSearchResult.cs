namespace InventoryManager.Models.ViewModels.Search {

    public class ItemSearchResult {
        public int Id { get; set; }
        public string CustomId { get; set; } = null!;
        public int InventoryId { get; set; }
        public string InventoryName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
