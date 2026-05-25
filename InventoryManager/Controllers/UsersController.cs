using InventoryManager.Services;
using InventoryManager.Services.Mappers;
using InventoryManager.Utilities.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers {

    public class UsersController(IUserService userService) : Controller {

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(PagedRequest pagedRequest) {
            var users = await userService.Get(pagedRequest);
            return View(users.ToViewModel());
        }
    }
}
