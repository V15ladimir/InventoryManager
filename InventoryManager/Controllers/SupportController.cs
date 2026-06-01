using InventoryManager.Integration.PowerAutomate.Models;
using InventoryManager.Models.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers {

    [Authorize]
    public class SupportController : Controller {

        public async Task<IActionResult> Index() {
            return View(new SupportTicketModel());
        }
    }
}
