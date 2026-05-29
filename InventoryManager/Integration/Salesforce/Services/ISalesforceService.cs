using InventoryManager.Integration.Salesforce.Models;

namespace InventoryManager.Integration.Salesforce.Services {

    public interface ISalesforceService {
        Task ExportAsync(SalesforceExportModel export);
    }
}
