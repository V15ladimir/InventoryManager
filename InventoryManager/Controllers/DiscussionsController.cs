using InventoryManager.Hubs;
using InventoryManager.Models.Entitites;
using InventoryManager.Models.ViewModels.Discussions;
using InventoryManager.Services;
using InventoryManager.Services.Mappers;
using InventoryManager.Utilities.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace InventoryManager.Controllers {

    public class DiscussionsController(
        UserManager<ApplicationUser> userManager, 
        IHubContext<DiscussionHub> hubContext,
        IDiscussionService discussionService) : Controller {

        [HttpGet]
        public async Task<IActionResult> GetDiscussions(int inventoryId, PagedRequest pagedRequest) {
            var discussions = await discussionService.GetDiscussionsAsync(inventoryId, pagedRequest);
            return Ok(discussions.ToViewModel());
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateInventoryDicussionViewModel discussionModel) {
            var discussion = await discussionService.CreateDiscussionAsync(discussionModel.ToDto(userManager.GetUserId(User)));
            await hubContext.Clients.Group($"inventory-{discussionModel.InventoryId}")
                .SendAsync("ReceiveMessage", discussion.ToViewModel());
            return Ok();
        }
    }
}
