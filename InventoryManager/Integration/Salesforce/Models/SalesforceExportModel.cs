namespace InventoryManager.Integration.Salesforce.Models {

    public sealed class SalesforceExportModel {
        public SalesforceCompanyModel Company { get; set; } = new();
        public SalesforceAccountModel Account { get; set; } = new();
    }

    public sealed class SalesforceCompanyModel {
        public string CompanyName { get; set; } = string.Empty;
        public string? CompanyType { get; set; }
        public string? CompanyPhone { get; set; }
        public string? CompanyWebSite { get; set; }
        public string? CompanySite { get; set; }
        public string? Industry { get; set; }
    }

    public sealed class SalesforceAccountModel {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ContactPhone { get; set; }
        public string? MobilePhone { get; set; }
        public string? Title { get; set; }
    }
}
