namespace InventoryManager.Models.ViewModels.Search {

    public class SearchData {
        public string Query { get; set; } = null!;
        public List<InventorySearchResult> Inventories { get; set; } = [];
        public List<ItemSearchResult> Items { get; set; } = [];
        public int TotalCount => Inventories.Count + Items.Count;
    }
}
