using InventoryManager.Integration.PowerAutomate.Models;
using InventoryManager.Integration.PowerAutomate.Services;
using InventoryManager.Integration.Salesforce.Models;
using InventoryManager.Integration.Salesforce.Services;

namespace InventoryManager.Extensions {

    public static class IntegrationServicesExtensions {

        public static IServiceCollection AddIntegrationServices(
            this IServiceCollection services,
            IConfiguration configuration) {
            services.Configure<SalesforceOptions>(configuration.GetSection("Salesforce"));
            services.AddHttpClient<ISalesforceService, SalesforceService>();
            services.Configure<DropboxOptions>(configuration.GetSection("Dropbox"));
            services.AddScoped<IDropBoxService, DropboxService>();
            return services;
        }
    }
}
