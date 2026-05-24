namespace InventoryManager.Utilities.Pagination {

    public record PagedRequest(
        string? SortBy = "created", 
        string? SortOrder = "desc", 
        int Page = 1, 
        int PageSize = 10,
        string? SearchText = "") {
        public int Offset => (Page - 1) * PageSize;
    }
}
