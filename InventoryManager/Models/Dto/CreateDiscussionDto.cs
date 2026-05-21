namespace InventoryManager.Models.Dto {

    public class CreateDiscussionDto {
        public int InventoryId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
