namespace InventoryManager.Integration.PowerAutomate.Models {

    public class DropboxOptions {
        public string AppKey { get; set; } = string.Empty;
        public string AppSecret { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string TargetFolder { get; set; } = string.Empty;
    }
}
