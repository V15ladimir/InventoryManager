using InventoryManager.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Extensions {

    public static class DatabaseServiceExtensions {

        public static IServiceCollection AddDatabaseServices(
            this IServiceCollection services,
            IConfiguration configuration) {
            var connectionString = GetConnectionString(configuration);
            services.AddDbContext<ApplicationDbContext>(x => x.UseNpgsql(connectionString));
            services.AddScoped<ApplicationDbInitializer>();
            return services;
        }

        private static string GetConnectionString(IConfiguration configuration) {
            return Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                ?? configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }
    }
}
