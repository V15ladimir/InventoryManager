namespace InventoryManager.Models.ViewModels.Items {

    public class ItemFieldValuesViewModel {
        public int Id { get; set; }
        public string CustomId { get; set; } = string.Empty;
        public IEnumerable<ItemFieldValueViewModel> ItemValues { get; set; } = [];
    }

    public class ItemFieldValueViewModel {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public string? Value { get; set; }
    }
}
