namespace InventoryManager.Models.ViewModels.Inventories {

    public class InventoryViewModel {
        public InventoryBasicViewModel Basic { get; set; } = new();
        public InventoryCategViewModel Category { get; set; } = new();
        public InventoryStatusViewModel Status { get; set; } = new();
        public InventoryAuditViewModel Audit { get; set; } = new();
    }

    public class InventoryBasicViewModel {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class InventoryAuditViewModel {
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class InventoryCategViewModel{
        public string CategoryName { get; set; } = string.Empty;
    }

    public class InventoryStatusViewModel {
        public bool IsPublic { get; set; }
    }
}
