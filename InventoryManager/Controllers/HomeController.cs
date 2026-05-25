using System.Diagnostics;
using InventoryManager.Models;
using InventoryManager.Models.ViewModels.Home;
using InventoryManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers {

    public class HomeController(IInventoryService inventoryService) : Controller {

        [HttpGet]
        public async Task<IActionResult> Index() {
            var latestInventories = await inventoryService.GetLatestInventories();
            var topInventories = await inventoryService.GetTopInventories();
            return View(new HomeIndexViewModel { 
                LatestInventories = latestInventories,
                TopInventories = topInventories
            });
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
