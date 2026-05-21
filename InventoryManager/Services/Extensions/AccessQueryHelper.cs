using InventoryManager.Models.Entitites;
using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Services.Extensions {

    public static class AccessQueryHelper {

        public static IQueryable<ApplicationUser> ApplyFilters(
            this IQueryable<ApplicationUser> query, 
            PagedRequest request) {
            if(string.IsNullOrWhiteSpace(request.SearchText))
                return query;
            return query.Where(x =>
                EF.Functions.ILike(x.UserName!, $"{request.SearchText}%") ||
                EF.Functions.ILike(x.Email!, $"{request.SearchText}%"));
        }

        public static IQueryable<ApplicationUser> ApplySorting(
            this IQueryable<ApplicationUser> query,
            PagedRequest pagedRequest,
            Dictionary<string, InventoryAccess> access) {
            if(string.IsNullOrWhiteSpace(pagedRequest.SortBy)) {
                return query;
            }
            return pagedRequest.SortBy.ToLower() switch {
                "access" => ApplyAccessSort(query, pagedRequest, access),
                "username" => ApplyUsernameSort(query, pagedRequest),
                "email" => ApplyEmailSort(query, pagedRequest),
                "created" => ApplyCreatedSort(query, pagedRequest),
                _ => query.OrderBy(x => x.UserName)
            };
        }

        private static IQueryable<ApplicationUser> ApplyAccessSort(
            IQueryable<ApplicationUser> query,
            PagedRequest pagedRequest,
            Dictionary<string, InventoryAccess> access) {
            return pagedRequest.SortOrder == "asc" ?
                query.OrderBy(x => access.ContainsKey(x.Id))
                    .ThenBy(x => x.UserName) :
                query.OrderByDescending(x => access.ContainsKey(x.Id))
                    .ThenBy(x => x.Email);
        }

        private static IQueryable<ApplicationUser> ApplyUsernameSort(
            IQueryable<ApplicationUser> query,
            PagedRequest pagedRequest) {
            return pagedRequest.SortOrder == "asc" ?
                query.OrderBy(x => x.UserName) :
                query.OrderByDescending(x => x.UserName);
        }

        private static IQueryable<ApplicationUser> ApplyEmailSort(
            IQueryable<ApplicationUser> query,
            PagedRequest pagedRequest) {
            return pagedRequest.SortOrder == "asc" ?
                query.OrderBy(x => x.Email) :
                query.OrderByDescending(x => x.Email);
        }

        private static IQueryable<ApplicationUser> ApplyCreatedSort(
            IQueryable<ApplicationUser> query,
            PagedRequest pagedRequest) {
            return pagedRequest.SortOrder == "asc" ?
                query.OrderBy(x => x.CreatedAt) :
                query.OrderByDescending(x => x.CreatedAt);
        }
    }
}
