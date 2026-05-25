using InventoryManager.Models.Dto;
using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Services {

    public interface IUserService {
        Task<PagedList<UserDto>> Get(PagedRequest pagedRequest);
    }
}
