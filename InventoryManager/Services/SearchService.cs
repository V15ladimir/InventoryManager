using InventoryManager.Data;
using InventoryManager.Models.ViewModels.Search;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Services {

    public class SearchService(ApplicationDbContext context) : ISearchService {

        public async Task<SearchData> Search(string searchText) {
            if(string.IsNullOrWhiteSpace(searchText)) {
                return new SearchData { Query = searchText };
            }

            var result = new SearchData { Query = searchText };
            result.Inventories = await context.Inventories
                .Where(x => 
                    x.SearchVectorEn.Matches(EF.Functions.PlainToTsQuery("english", searchText)) || 
                    x.SearchVectorRu.Matches(EF.Functions.PlainToTsQuery("russian", searchText)))
                .Select(x => new InventorySearchResult {
                    InventoryId = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CategoryName = x.Category.Name,
                    CreatedBy = x.CreatedBy != null ? $"{x.CreatedBy.FirstName} {x.CreatedBy.LastName}" : "Unknown",
                    CreatedAt = x.CreatedAt
                })
                .Take(50)
                .ToListAsync();

            result.Items = await context.Items
                .Where(x => 
                    x.SearchVectorEn.Matches(EF.Functions.PlainToTsQuery("english", searchText)) || 
                    x.SearchVectorRu.Matches(EF.Functions.PlainToTsQuery("russian", searchText)))
                .Select(x => new ItemSearchResult {
                    Id = x.Id,
                    CustomId = x.CustomId,
                    InventoryId = x.InventoryId,
                    InventoryName = x.Inventory.Name,
                    CreatedAt = x.CreatedAt
                })
                .Take(50)
                .ToListAsync();
            return result;
        }
    }
}
