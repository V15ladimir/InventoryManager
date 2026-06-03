using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication.Google;

namespace InventoryManager.Extensions {
    public static class AuthenticationServiceExtensions {

        public static IServiceCollection AddExternalAuthentication(this IServiceCollection services) {
            services.AddAuthentication()
                .AddGoogle(ConfigureGoogleAuth)
                .AddGitHub(ConfigureGitHubAuth);
            return services;
        }

        private static void ConfigureGoogleAuth(GoogleOptions options) {
            options.ClientId = Environment.GetEnvironmentVariable("Authentication__Google__ClientId")
                ?? throw new InvalidOperationException("Google ClientId not found");
            options.ClientSecret = Environment.GetEnvironmentVariable("Authentication__Google__ClientSecret")
                ?? throw new InvalidOperationException("Google ClientSecret not found");
        }

        private static void ConfigureGitHubAuth(GitHubAuthenticationOptions options) {
            options.ClientId = Environment.GetEnvironmentVariable("Authentication__GitHub__ClientId")
                ?? throw new InvalidOperationException("GitHub ClientId not found");
            options.ClientSecret = Environment.GetEnvironmentVariable("Authentication__GitHub__ClientSecret")
                ?? throw new InvalidOperationException("GitHub ClientSecret not found");
            options.Scope.Add("user:email");
            options.SaveTokens = true;
        }
    }
}
