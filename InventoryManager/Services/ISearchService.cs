using InventoryManager.Models.ViewModels.Search;

namespace InventoryManager.Services {

    public interface ISearchService {
        Task<SearchData> Search(string searchText);
    }
}
