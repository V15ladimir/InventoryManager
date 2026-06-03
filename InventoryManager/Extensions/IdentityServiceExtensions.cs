using InventoryManager.Data;
using InventoryManager.Models.Entitites;
using Microsoft.AspNetCore.Identity;

namespace InventoryManager.Extensions {

    public static class IdentityServiceExtensions {

        public static IServiceCollection AddIdentityServices(this IServiceCollection services) {
            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();
            return services;
        }
    }
}
