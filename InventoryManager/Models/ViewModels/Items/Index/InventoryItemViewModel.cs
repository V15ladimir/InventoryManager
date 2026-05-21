namespace InventoryManager.Models.ViewModels.Items.Index {

    public class InventoryItemViewModel {
        public int InventoryId { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? SearchText { get; set; }
    }
}
