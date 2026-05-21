using InventoryManager.Models.ViewModels.Inventories.Shared;
using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Models.ViewModels.Items.Form {

    public class ItemFormViewModel {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public string? CustomId { get; set; }
        public List<InventoryFieldViewModel> Fields { get; set; } = [];
        public Dictionary<int, string?> FieldValues { get; set; } = [];
        public PagedRequest PagedRequest { get; set; } = new();
    }
}
