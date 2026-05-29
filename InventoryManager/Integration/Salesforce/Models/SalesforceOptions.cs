namespace InventoryManager.Integration.Salesforce.Models {

    public class SalesforceOptions {
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
        public required string LoginUrl { get; set; }
        public required string ApiVersion { get; set; }
    }
}
