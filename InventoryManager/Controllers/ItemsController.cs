using InventoryManager.Models.Entitites;
using InventoryManager.Models.ViewModels.Items;
using InventoryManager.Services;
using InventoryManager.Services.Mappers;
using InventoryManager.Utilities.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers {

    public class ItemsController(
        UserManager<ApplicationUser> userManager, 
        IItemService itemService, 
        IAccessService accessService) : Controller {

        [HttpGet]
        public async Task<IActionResult> Index(int inventoryId, PagedRequest pagedRequest) {
            var canEdit = await accessService.CanEditItemsAsync(inventoryId, userManager.GetUserId(User));
            var items = await itemService.GetItemsAsync(inventoryId, pagedRequest);
            return PartialView("_InventoryItems", items.ToViewModel(canEdit));
        }

        [HttpGet]
        public async Task<IActionResult> Create(int inventoryId, PagedRequest pagedRequest) {
            var customId = await itemService.GetItemCustomIdAsync(inventoryId);
            var fields = await itemService.GetItemFieldsAsync(inventoryId);
            return PartialView("Form", fields.ToViewModel(
                0,
                inventoryId, 
                customId, 
                pagedRequest,
                []
            ));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int itemId, PagedRequest pagedRequest) {
            var item = await itemService.GetItemAsync(itemId);
            var fields = await itemService.GetItemFieldsAsync(item.InventoryId);
            return PartialView("Form", fields.ToViewModel(
                item.ItemId,
                item.InventoryId,
                item.CustomId,
                pagedRequest,
                await itemService.GetItemValuesAsync(itemId)
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
        [Authorize]
        public async Task<IActionResult> Create(ItemFormViewModel item) {
            await itemService.CreateItemAsync(item.ToDto());
            var canEdit = await accessService.CanEditItemsAsync(item.InventoryId, userManager.GetUserId(User));
            var items = await itemService.GetItemsAsync(item.InventoryId, item.PagedRequest);
            return PartialView("_InventoryItems", items.ToViewModel(canEdit));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(ItemFormViewModel item) {
            await itemService.UpdateItemAsync(item.ToUpdateDto());
            var canEdit = await accessService.CanEditItemsAsync(item.InventoryId, userManager.GetUserId(User));
            var items = await itemService.GetItemsAsync(item.InventoryId, item.PagedRequest);
            return PartialView("_InventoryItems", items.ToViewModel(canEdit));
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int inventoryId, List<int> itemIds) {
            await itemService.DeleteItemsAsync(inventoryId, itemIds);
            var canEdit = await accessService.CanEditItemsAsync(inventoryId, userManager.GetUserId(User));
            var items = await itemService.GetItemsAsync(inventoryId, new PagedRequest());
            return PartialView("_InventoryItems", items.ToViewModel(canEdit));
        }
    }
}

