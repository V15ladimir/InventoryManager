namespace InventoryManager.Models.Dto {

    public class InventoryDiscussionDto {
        public int DiscussionId { get; set; }
        public string DiscussionContent { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
