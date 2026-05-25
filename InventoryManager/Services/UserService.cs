using InventoryManager.Data;
using InventoryManager.Models.Dto;
using InventoryManager.Services.Mappers;
using InventoryManager.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Services {

    public class UserService(ApplicationDbContext context) : IUserService {

        public async Task<PagedList<UserDto>> Get(PagedRequest pagedRequest) {
            return await context.Users.AsNoTracking()
                .Select(x => x.ToDto(false, x.LockoutEnabled))
                .ToPagedResponseAsync(pagedRequest);
        }
    }
}
