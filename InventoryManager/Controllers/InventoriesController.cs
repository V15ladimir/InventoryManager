using FluentValidation;
using InventoryManager.Models.Entitites;
using InventoryManager.Models.Enums;
using InventoryManager.Models.ViewModels.Inventories.Form;
using InventoryManager.Models.ViewModels.Inventories.Index;
using InventoryManager.Models.ViewModels.Inventories.Shared;
using InventoryManager.Services;
using InventoryManager.Services.Extensions;
using InventoryManager.Services.Mappers;
using InventoryManager.Utilities.Pagination;
using InventoryManager.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers {

    public class InventoriesController(
        UserManager<ApplicationUser> userManager, 
        IInventoryService inventoryService,
        ICategoryService categoryService,
        IValidator<InventorySettingsViewModel> settingsValidator,
        IValidator<InventoryCustomIdPartsViewModel> customIdPartsValidator,
        IValidator<InventoryCustomFieldsViewModel> customFieldsValidator) : Controller {

        [HttpGet]
        public async Task<IActionResult> Index(ViewType viewType, PagedRequest pagedRequest) {
            var inventories = viewType switch {
                ViewType.My => await inventoryService.GetMyInventoriesAsync(userManager.GetUserId(User), pagedRequest),
                ViewType.Shared => await inventoryService.GetSharedInventoriesAsync(userManager.GetUserId(User), pagedRequest),
                _ => await inventoryService.GetInventoriesAsync(pagedRequest)
            };
            return View(new InventoriesIndexViewModel {
                ViewType = viewType,
                Inventories = inventories
            });
        }

        [HttpGet]
        public async Task<IActionResult> Create(PagedRequest pagedRequest) {
            var categories = await categoryService.GetCategoriesAsync();
            return View("Create", InventoryMapper.GetInventorySettingsViewModel(
                categories, 
                pagedRequest));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int inventoryId, PagedRequest pagedRequest) {
            var inventory = await inventoryService.GetInventoryAsync(inventoryId);
            var categories = await categoryService.GetCategoriesAsync();
            var parts = await inventoryService.GetInventoryIdPartsAsync(inventoryId);
            var fields = await inventoryService.GetInventoryFieldsAsync(inventoryId);
            return View(inventory.ToItemsIndexViewModel(
                categories, 
                parts, 
                fields, 
                pagedRequest));
        }

        [HttpGet]
        public async Task<IActionResult> SearchAccess(int inventoryId, PagedRequest pagedRequest) {
            var access = await inventoryService.GetInventoryAccessAsync(inventoryId, pagedRequest);
            return View("Access", access.ToListViewModel(inventoryId));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateInventory(InventorySettingsViewModel settings) {
            var validation = await settingsValidator.ValidateAsync(settings);
            var categories = await categoryService.GetCategoriesAsync();
            settings.Categories = categories.ToViewModel();
            if(!validation.IsValid)
                validation.AddToModelState(this.ModelState);
            if(!ModelState.IsValid)
                return PartialView("_InventorySettings", settings);
            await inventoryService.CreateInventoryAsync(settings.Details.ToCreateInventoryDto(userManager.GetUserId(User)));
            Response.Headers.Append("HX-Redirect", Url.Action("Index"));
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateInventory(int inventoryId, InventorySettingsViewModel settings) {
            var validation = await settingsValidator.ValidateAsync(settings);
            var categories = await categoryService.GetCategoriesAsync();
            settings.Categories = categories.ToViewModel();
            if(!validation.IsValid)
                validation.AddToModelState(this.ModelState);
            if(!ModelState.IsValid)
                return PartialView("_InventorySettings", settings);
            await inventoryService.UpdateInventoryAsync(settings.Details.ToUpdateInventoryDto(inventoryId, userManager.GetUserId(User)));
            return PartialView("_InventorySettings", settings);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateCustomIdParts(InventoryCustomIdPartsViewModel parts) {
            var validation = await customIdPartsValidator.ValidateAsync(parts);
            if(!validation.IsValid)
                validation.AddToModelState(this.ModelState);
            if(!ModelState.IsValid)
                return BadRequest(PartialView("_CustomIdParts", parts));
            await inventoryService.UpdateCustomIdPartsAsync(parts.InventoryId, parts);
            return PartialView("_CustomIdParts", parts);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateCustomFields(InventoryCustomFieldsViewModel fields) {
            var validation = await customFieldsValidator.ValidateAsync(fields);
            if(!validation.IsValid)
                validation.AddToModelState(this.ModelState);
            if(!ModelState.IsValid)
                return BadRequest(PartialView("_CustomFields", fields));
            await inventoryService.UpdateCustomFieldsAsync(fields.InventoryId, fields);
            return PartialView("_CustomFields", fields);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateAccess(
            int inventoryId, 
            InventoryAccessViewModel acces, 
            PagedRequest pagedRequest) {
            await inventoryService.UpdateInventoryAccessAsync(acces.ToUpdateInventoryAccessDto(inventoryId));
            var access = await inventoryService.GetInventoryAccessAsync(inventoryId, pagedRequest);
            return View("Access", access.ToListViewModel(inventoryId));
        }

        [HttpPost]
        public async Task<IActionResult> PreviewCustomId(InventoryFormViewModel model) {
            var preview = string.Concat(model.CustomIdParts.Select(x => x.BuildPreview()));
            return Content(preview);  
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(List<int> inventoryIds, PagedRequest pagedRequest) {
            await inventoryService.DeleteInventoryAsync(inventoryIds);
            return RedirectToAction("Index", pagedRequest);
        }

        [HttpPost]
        public async Task<IActionResult> GetCustomIdPartHtml(InventoryCustomIdPartTypeViewModel part) {
            return PartialView("_CustomIdPart", new InventoryIdPartViewModel { Type = part.Type });
        }

        [HttpPost]
        public async Task<IActionResult> GetCustomFieldHtml(InventoryCustomFieldTypeViewModel field) {
            return PartialView("_CustomField", new InventoryFieldViewModel { Type = field.Type });
        }
    }
}
