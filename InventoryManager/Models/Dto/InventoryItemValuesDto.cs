namespace InventoryManager.Models.Dto {

    public class InventoryItemValuesDto {
        public int ItemId { get; set; }
        public int InventoryId { get; set; }
        public string CustomId { get; set; } = string.Empty;
        public List<InventoryItemValueDto> ItemValues { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
