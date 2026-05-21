using InventoryManager.Models.Entitites;
using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Models.Entitites.Items;
using InventoryManager.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace InventoryManager.Services.Extensions {
    public static class ItemQueryHelper {

        public static IQueryable<Item> ApplyFilters(
            this IQueryable<Item> items,
            PagedRequest request) {
            if(string.IsNullOrWhiteSpace(request.SearchText))
                return items;
            return items.Where(x => 
                x.SearchVectorEn.Matches(EF.Functions.PlainToTsQuery("english", request.SearchText)) || 
                EF.Functions.ILike(x.CustomId, $"{request.SearchText}%"));
        }

        public static IQueryable<Item> ApplySorting(
            this IQueryable<Item> query,
            PagedRequest pagedRequest) {
            if(string.IsNullOrWhiteSpace(pagedRequest.SortBy)) 
                return query;
            return pagedRequest.SortBy.ToLower() switch {
                "created" => ApplyCreatedSort(query, pagedRequest),
                "updated" => ApplyUpdatedSort(query, pagedRequest),
                _ => query.OrderBy(x => x.CreatedAt)
            };
        }

        private static IQueryable<Item> ApplyCreatedSort(
            IQueryable<Item> query,
            PagedRequest pagedRequest) {
            return pagedRequest.SortOrder == "asc" ?
                query.OrderBy(x => x.CreatedAt) :
                query.OrderByDescending(x => x.CreatedAt);
        }

        private static IQueryable<Item> ApplyUpdatedSort(
            IQueryable<Item> query,
            PagedRequest pagedRequest) {
            return pagedRequest.SortOrder == "asc" ?
                query.OrderBy(x => x.UpdatedAt) :
                query.OrderByDescending(x => x.UpdatedAt);
        }
    }
}
