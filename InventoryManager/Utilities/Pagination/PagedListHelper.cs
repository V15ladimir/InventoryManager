using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Utilities.Pagination {

    public static class PagedListHelper {
        public static async Task<PagedList<T>> ToPagedResponseAsync<T>(this IQueryable<T> query, PagedRequest pagedRequest) {
            var count = await query.CountAsync();
            var elements = await query
                .Skip(pagedRequest.Offset)
                .Take(pagedRequest.PageSize)
                .ToListAsync();
            return new PagedList<T>(
                elements,
                pagedRequest.Page,
                pagedRequest.PageSize,
                count,
                pagedRequest.SortBy,
                pagedRequest.SortOrder,
                pagedRequest.SearchText
            );
        }

        public static PagedList<T> CreateEmpty<T>(PagedRequest pagedRequest) {
            return new PagedList<T>(
                [],
                pagedRequest.Page,
                pagedRequest.PageSize,
                0,
                pagedRequest.SortBy,
                pagedRequest.SortOrder,
                pagedRequest.SearchText
            );
        }
    }
}
