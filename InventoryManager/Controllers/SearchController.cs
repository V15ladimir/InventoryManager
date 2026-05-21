using InventoryManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers {

    public class SearchController(ISearchService searchService) : Controller {

        [HttpGet]
        public async Task<IActionResult> Index(string? globalSearch) {
            var result = await searchService.Search(globalSearch ?? string.Empty);
            return View(result);
        }
    }
}
