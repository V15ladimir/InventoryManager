using InventoryManager.Data;
using InventoryManager.Exceptions;
using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites;
using InventoryManager.Services.Mappers;
using InventoryManager.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Services {

    public class DiscussionService(ApplicationDbContext context) : IDiscussionService {

        public async Task<InventoryDiscussionDto> CreateDiscussionAsync(CreateDiscussionDto discussionDto) {
            var discussion = discussionDto.ToEntity();
            await context.InventoryDiscussions.AddAsync(discussionDto.ToEntity());
            await context.SaveChangesAsync();
            return discussion.ToDto(await GetUserAsync(discussionDto.UserId));
        }

        public async Task<PagedList<InventoryDiscussionDto>> GetDiscussionsAsync(int inventoryId, PagedRequest pagedRequest) {
            return await context.InventoryDiscussions
                .AsNoTracking()
                .Where(x => x.InventoryId == inventoryId)
                .OrderBy(x => x.CreatedAt)
                .Select(x => x.ToDto(x.CreatedBy))
                .ToPagedResponseAsync(pagedRequest);
        }

        private async Task<ApplicationUser> GetUserAsync(string? userId) {
            return await context.Users.FindAsync(userId) ?? throw new NotFoundException("User not found");
        }
    }
}
