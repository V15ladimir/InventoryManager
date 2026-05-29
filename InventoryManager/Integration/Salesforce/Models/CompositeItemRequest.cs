namespace InventoryManager.Integration.Salesforce.Models {

    public class CompositeItemRequest {
        public string Method { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string ReferenceId { get; set; } = string.Empty;
        public object Body { get; set; } = new();
    }
}
