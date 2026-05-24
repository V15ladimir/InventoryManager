using InventoryManager.Data;
using InventoryManager.Models.Entitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Services {

    public class AccessService(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : IAccessService {

        public async Task<bool> CanEditInventoryAsync(int inventoryId, string? userId) {
            if(string.IsNullOrEmpty(userId))
                return false;
            if(await IsAdminAsync(userId))
                return true;
            return await IsInventoryOwnerAsync(inventoryId, userId);
        }

        public async Task<bool> CanEditItemsAsync(int inventoryId, string? userId) {
            if(string.IsNullOrEmpty(userId))
                return false;
            if(await CanEditInventoryAsync(inventoryId, userId)) {
                return true;
            }
            return await IsInventoryWriterAsync(inventoryId, userId);
        }

        private async Task<ApplicationUser?> GetUser(string userId) => await userManager.FindByIdAsync(userId);

        private async Task<bool> IsAdminAsync(string userId) {
            var user = await GetUser(userId);
            return user != null && await userManager.IsInRoleAsync(user, "Admin");
        }

        private async Task<bool> IsInventoryOwnerAsync(int inventoryId, string userId) {
            return await context.Inventories.AnyAsync(x => x.Id == inventoryId && x.CreatedById == userId);
        }

        private async Task<bool> IsInventoryWriterAsync(int inventoryId, string userId) {
            return await context.Inventories.Where(x => x.Id == inventoryId)
                .Select(x => x.IsPublic || x.AccessList.Any(x => x.UserId == userId))
                .FirstOrDefaultAsync();
        }
    }
}
