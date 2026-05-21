namespace InventoryManager.Utilities.Pagination {

    public class PagedList<T>(
        List<T> elements, 
        int pageIndex = 1, 
        int pageSize = 20, 
        int totalCount = 0, 
        string? sortBy = null, 
        string? sortOrder = null, 
        string? searchText = null) {
        public List<T> Elements { get; set; } = elements;
        public int PageIndex { get; set; } = pageIndex;
        public int PageSize { get; set; } = pageSize;
        public int TotalCount { get; set; } = totalCount;
        public string? SortBy { get; set; } = sortBy;
        public string? SortOrder { get; set; } = sortOrder;
        public string? SearchText { get; set; } = searchText;
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
