namespace InventoryManager.Utilities.Pagination {

    public record PagedRequest(
        string? SortBy = "Created", 
        string? SortOrder = "desc", 
        int Page = 1, 
        int PageSize = 20,
        string? SearchText = ""
    ) {
        public int Offset => (Page - 1) * PageSize;
    }
}
