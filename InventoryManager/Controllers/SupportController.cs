using InventoryManager.Integration.PowerAutomate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers {

    [Authorize]
    public class SupportController : Controller {

        public async Task<IActionResult> Index(string link, string inventory) {
            var supportTicket = new SupportTicketModel {
                ReportedBy = User?.Identity?.Name ?? "Anonymous",
                Link = link,
                Inventory = inventory
            };
            return View(supportTicket);
        }
    }
}
