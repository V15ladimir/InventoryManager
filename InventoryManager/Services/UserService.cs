using InventoryManager.Data;
using InventoryManager.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Services {

    public class UserService(ApplicationDbContext context) {

        public async void Get(PagedRequest pagedRequest) {
            await context.Users.AsNoTracking().ToPagedResponseAsync(pagedRequest);
        }
    }
}
