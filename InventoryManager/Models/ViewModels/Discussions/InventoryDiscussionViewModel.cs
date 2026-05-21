namespace InventoryManager.Models.ViewModels.Discussions {

    public class InventoryDiscussionViewModel {
        public InventoryDiscussionBasicViewModel Basic { get; set; } = new();
        public InventoryDiscussionAuthorViewModel Author { get; set; } = new();
        public InventoryDiscussionAuditViewModel Audit { get; set; } = new();
    }

    public class InventoryDiscussionBasicViewModel {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    public record InventoryDiscussionAuthorViewModel {
        public string AuthorId { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
    }

    public class InventoryDiscussionAuditViewModel { 
        public DateTime CreatedAt { get; set; }
    }
}
