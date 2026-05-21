using InventoryManager.Models.Enums;

namespace InventoryManager.Models.ViewModels.Inventories.Form {

    //public record InventoryIdPartViewModel(
    //    int Order,
    //    string? Text, 
    //    string? Format,
    //    int? NumberWidth,
    //    bool? UseHex,
    //    IdType Type
    //);

    public class InventoryIdPartViewModel {
        public int Id { get; set; }
        public int Order { get; set; }
        public string? Text { get; set; }
        public string? Format { get; set; }
        public int? NumberWidth { get; set; }
        public bool? UseHex { get; set; }
        public IdType Type { get; set; }
    }
}
