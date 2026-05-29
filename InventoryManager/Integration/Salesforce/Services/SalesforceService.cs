using System.Net.Http.Headers;
using InventoryManager.Integration.Salesforce.Models;
using Microsoft.Extensions.Options;

namespace InventoryManager.Integration.Salesforce.Services {

    public class SalesforceService(
        HttpClient httpClient, 
        IOptions<SalesforceOptions> options) : ISalesforceService {

        public async Task ExportAsync(SalesforceExportModel export) {
            var auth = await AutheticateAsync();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);
            var compositeRequest = new {
                compositeRequest = new[] {
                    CreateCompany(export.Company),
                    CreateAccount(export.Account)
                }
            };

            var response = await httpClient.PostAsJsonAsync($"{auth.InstanceUrl}/services/data/{options.Value.ApiVersion}/composite", compositeRequest);
            response.EnsureSuccessStatusCode();
        }

        private CompositeItemRequest CreateCompany(SalesforceCompanyModel company) {
            return new CompositeItemRequest {
                Method = "POST",
                Url = "/services/data/v66.0/sobjects/Account",
                ReferenceId = "newAccount",
                Body = new {
                    Name = company.CompanyName,
                    Site = company.CompanySite,
                    Type = company.CompanyType,
                    Phone = company.CompanyPhone,
                    Website = company.CompanyWebSite,
                    company.Industry
                }
            };
        }

        private CompositeItemRequest CreateAccount(SalesforceAccountModel account) {
            return new CompositeItemRequest {
                Method = "POST",
                Url = "/services/data/v66.0/sobjects/Contact",
                ReferenceId = "newContact",
                Body = new {
                    account.Title,
                    account.FirstName,
                    account.LastName,
                    account.Email,
                    Phone = account.ContactPhone,
                    account.MobilePhone,
                    AccountId = "@{newAccount.id}"
                }
            };
        }

        private async Task<SalesforceAuthResponse> AutheticateAsync() {
            var parameters = new Dictionary<string, string> {
                { "grant_type", "client_credentials" },
                { "client_id", options.Value.ClientId },
                { "client_secret", options.Value.ClientSecret }
            };

            var messageResponse = await httpClient.PostAsync(
                $"{options.Value.LoginUrl}/services/oauth2/token",
                new FormUrlEncodedContent(parameters));
            
            messageResponse.EnsureSuccessStatusCode();
            var authResponse = await messageResponse.Content.ReadFromJsonAsync<SalesforceAuthResponse>();
            return authResponse!;
        }
    }
}
