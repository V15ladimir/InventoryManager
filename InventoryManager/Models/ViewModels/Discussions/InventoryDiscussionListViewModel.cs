namespace InventoryManager.Models.ViewModels.Discussions {

    public class InventoryDiscussionListViewModel {
        public List<InventoryDiscussionViewModel> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasMore { get; set; }
    }
}
