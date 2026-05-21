namespace InventoryManager.Models.Dto {

    public class UpdateInventoryAccessDto {
        public int InventoryId { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool HasAccess { get; set; }
    }
}
