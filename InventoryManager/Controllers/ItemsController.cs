using InventoryManager.Models.ViewModels.Items.Form;
using InventoryManager.Services;
using InventoryManager.Services.Mappers;
using InventoryManager.Utilities.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers {

    public class ItemsController(IItemService itemService) : Controller {

        [HttpGet]
        public async Task<IActionResult> Index(int inventoryId, PagedRequest pagedRequest) {
            var items = await itemService.GetItemsAsync(inventoryId, pagedRequest);
            return View(items.ToViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Create(int inventoryId, PagedRequest pagedRequest) {
            var customId = await itemService.GetItemCustomIdAsync(inventoryId);
            var fields = await itemService.GetItemFieldsAsync(inventoryId);
            return View("Form", fields.ToViewModel(
                0,
                inventoryId, 
                customId, 
                pagedRequest,
                []
            ));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id, PagedRequest pagedRequest) {
            var item = await itemService.GetItemAsync(id);
            var fields = await itemService.GetItemFieldsAsync(item.InventoryId);
            return View("Form", fields.ToViewModel(
                item.ItemId,
                item.InventoryId,
                item.CustomId,
                pagedRequest,
                await itemService.GetItemValuesAsync(id)
            ));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, PagedRequest pagedRequest) {
            var item = await itemService.GetItemAsync(id);
            var fields = await itemService.GetItemFieldsAsync(item.InventoryId);
            return View("Details", fields.ToViewModel(
                item.ItemId,
                item.InventoryId,
                item.CustomId,
                pagedRequest,
                await itemService.GetItemValuesAsync(id)
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemFormViewModel item) {
            //return RedirectToAction("AccessDenied", "Home");
            await itemService.CreateItemAsync(item.ToDto());
            return RedirectToAction("Index", item.ToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Update(ItemFormViewModel item) {
            await itemService.UpdateItemAsync(item.ToUpdateDto());
            return RedirectToAction("Index", item.ToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int inventoryId, List<int> itemIds) {
            await itemService.DeleteItemsAsync(inventoryId, itemIds);
            return RedirectToAction("Index", new { inventoryId });
        }
    }
}

