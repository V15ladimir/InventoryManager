using InventoryManager.Models.ViewModels.Inventories.Shared;
using InventoryManager.Utilities.Pagination;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InventoryManager.Models.ViewModels.Inventories.Form {

    public record InventoryFormViewModel(
        InventorySettingsViewModel Settings,
        List<InventoryIdPartViewModel> CustomIdParts,
        List<InventoryFieldViewModel> CustomFields,
        List<InventoryAccessViewModel> AccessUsers
    );

    public class InventorySettingsViewModel {
        public int InventoryId { get; set; }
        public InventoryDetailsViewModel Details { get; set; } = null!;
        public PagedRequest PagedRequest { get; set; } = new();

        [ValidateNever]
        public List<InventoryCategoryViewModel> Categories { get; set; } = [];
    }

    public class InventoryCategoryViewModel {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    public class InventoryDetailsViewModel {
        public string InventoryName { get; set; } = "New inventory";
        public int InventoryCategoryId { get; set; } = 1;
        public bool IsPublicInventory { get; set; }
        public string? InventoryDescription { get; set; }
    }
}
