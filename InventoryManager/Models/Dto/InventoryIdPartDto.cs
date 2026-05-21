using InventoryManager.Models.Enums;

namespace InventoryManager.Models.Dto {

    public class InventoryIdPartDto {
        public int Id { get; set; }
        public int Order { get; set; }
        public string? Text { get; set; }
        public string? Format { get; set; }
        public int? NumberWidth { get; set; }
        public bool? UseHex { get; set; }
        public IdType Type { get; set; }
    }
}
