namespace InventoryManager.Models.ViewModels.Inventories.Shared {

    public record UpdateCustomFieldsViewModel(
        int InventoryId, 
        List<InventoryFieldViewModel> CustomFields,
        SaveStatusViewModel SaveStatus
    );
}
