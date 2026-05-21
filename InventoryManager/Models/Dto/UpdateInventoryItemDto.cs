namespace InventoryManager.Models.Dto {

    public class UpdateInventoryItemDto {
        public int ItemId { get; set; }
        public string? CustomId { get; set; }
        public Dictionary<int, string?> FieldValues { get; set; } = [];
    }
}
