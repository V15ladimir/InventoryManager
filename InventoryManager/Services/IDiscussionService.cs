using InventoryManager.Models.Dto;
using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Services {

    public interface IDiscussionService {
        Task<PagedList<InventoryDiscussionDto>> GetDiscussionsAsync(int inventoryId, PagedRequest pagedRequest);
        Task<InventoryDiscussionDto> CreateDiscussionAsync(CreateDiscussionDto discussionDto);
    }
}
