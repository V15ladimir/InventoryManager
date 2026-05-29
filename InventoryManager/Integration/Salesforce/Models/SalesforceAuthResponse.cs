using System.Text.Json.Serialization;

namespace InventoryManager.Integration.Salesforce.Models {

    public sealed class SalesforceAuthResponse {

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("instance_url")]
        public string InstanceUrl { get; set; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;
    }
}
