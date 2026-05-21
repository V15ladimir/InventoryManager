namespace InventoryManager.Models.Dto {

    public class CreateInventoryItemDto {
        public int InventoryId { get; set; }
        public string? CustomId { get; set; }
        public Dictionary<int, string?> FieldValues { get; set; } = [];
    }
}
