using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Services.Extensions {

    public static class InventoryQueryHelper {

        public static IQueryable<Inventory> ApplyFilters(
            this IQueryable<Inventory> inventories,
            PagedRequest request) {
            if(string.IsNullOrWhiteSpace(request.SearchText))
                return inventories;
            return inventories.Where(x => x.SearchVectorEn.Matches(EF.Functions.PlainToTsQuery("english", request.SearchText)));
        }

        public static IQueryable<Inventory> ApplySorting(
            this IQueryable<Inventory> query,
            PagedRequest pagedRequest) {
            if(string.IsNullOrWhiteSpace(pagedRequest.SortBy))
                return query;
            return pagedRequest.SortBy.ToLower() switch {
                "name" => ApplyNameSort(query, pagedRequest),
                "created" => ApplyCreatedSort(query, pagedRequest),
                "updated" => ApplyUpdatedSort(query, pagedRequest),
                _ => query.OrderBy(x => x.CreatedAt)
            };
        }

        private static IQueryable<Inventory> ApplyNameSort(
            IQueryable<Inventory> query,
            PagedRequest pagedRequest) {
            return pagedRequest.SortOrder == "asc" ?
                query.OrderBy(x => x.Name) :
                query.OrderByDescending(x => x.Name);
        }

        private static IQueryable<Inventory> ApplyCreatedSort(
            IQueryable<Inventory> query,
            PagedRequest pagedRequest) {
            return pagedRequest.SortOrder == "asc" ?
                query.OrderBy(x => x.CreatedAt) :
                query.OrderByDescending(x => x.CreatedAt);
        }

        private static IQueryable<Inventory> ApplyUpdatedSort(
            IQueryable<Inventory> query,
            PagedRequest pagedRequest) {
            return pagedRequest.SortOrder == "asc" ?
                query.OrderBy(x => x.UpdatedAt) :
                query.OrderByDescending(x => x.UpdatedAt);
        }
    }
}
