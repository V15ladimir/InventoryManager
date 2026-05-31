using System.Reflection.Metadata.Ecma335;
using InventoryManager.Models.Entitites;
using InventoryManager.Models.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers {

    [Authorize]
    public class ProfileController(UserManager<ApplicationUser> userManager) : Controller {

        public async Task<IActionResult> Index() {
            var user = await userManager.GetUserAsync(User);
            if(user == null) {
                return Challenge();
            }
            return View(new ProfileViewModel {
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty
            });
        }
    }
}
