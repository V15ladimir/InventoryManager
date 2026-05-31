using System.Net.Sockets;
using FluentValidation;
using InventoryManager.Data;
using InventoryManager.Hubs;
using InventoryManager.Integration.PowerAutomate.Models;
using InventoryManager.Integration.PowerAutomate.Services;
using InventoryManager.Integration.Salesforce.Models;
using InventoryManager.Integration.Salesforce.Services;
using InventoryManager.Models.Entitites;
using InventoryManager.Services;
using InventoryManager.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions => {
         googleOptions.ClientId = Environment.GetEnvironmentVariable("Authentication__Google__ClientId")
             ?? throw new InvalidOperationException("Google ClientId not found");
         googleOptions.ClientSecret = Environment.GetEnvironmentVariable("Authentication__Google__ClientSecret")
             ?? throw new InvalidOperationException("Google ClientSecret not found");
     })
    .AddGitHub(githubOptions => {
        githubOptions.ClientId = Environment.GetEnvironmentVariable("Authentication__GitHub__ClientId")
            ?? throw new InvalidOperationException("GitHub ClientId not found");
        githubOptions.ClientSecret = Environment.GetEnvironmentVariable("Authentication__GitHub__ClientSecret")
            ?? throw new InvalidOperationException("GitHub ClientSecret not found");
        githubOptions.Scope.Add("user:email");
        githubOptions.SaveTokens = true;
    });
builder.Services.AddControllersWithViews(options => {
    options.ModelValidatorProviders.Clear();
});
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found");
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseNpgsql(connectionString));
builder.Services.AddScoped<ApplicationDbInitializer>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IDiscussionService, DiscussionService>();
builder.Services.AddScoped<IAccessService, AccessService>();
builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.Configure<SalesforceOptions>(builder.Configuration.GetSection("Salesforce"));
builder.Services.AddHttpClient<ISalesforceService, SalesforceService>();
//builder.Services.Configure<DropboxOptions>(builder.Configuration.GetSection("Dropbox"));
builder.Services.AddScoped<IDropBoxService, DropboxService>();
builder.Services.AddValidatorsFromAssemblyContaining<InventorySettingsValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<InventoryCustomIdPartsValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<InventoryCustomFieldsValidator>();

var app = builder.Build();
app.Use((context, next) => {
    context.Request.Scheme = "https";
    return next();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

using(var scope = app.Services.CreateScope()) {
    await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>().InitializeAsync(scope.ServiceProvider);
    //var salesforceService = scope.ServiceProvider.GetRequiredService<ISalesforceService>();
    //var testModel = new SalesforceExportModel {
    //    Company = new SalesforceCompanyModel {
    //        CompanyName = "Test Company 2",
    //        CompanySite = "Головной офис",
    //        CompanyType = "Customer",
    //        CompanyPhone = "+1 (212) 555-1234",
    //        CompanyWebSite = "https://www.example.com",
    //        Industry = "Technology"
    //    },
    //    Account = new SalesforceAccountModel {
    //        Title = "Software Engineer",
    //        FirstName = "Иван",
    //        LastName = "Иван",
    //        Email = "ivan@example.com",
    //        ContactPhone = "+1 (212) 555-1234",
    //        MobilePhone = "+375291111111"
    //    }
    //};
    //await salesforceService.ExportAsync(testModel);
    //var test = new {
    //    Subject = "test",
    //    Priority = "test",
    //    ReportedBy = "Anonymous",
    //    Inventory = "N/A",
    //    Link = "https://api.nuget.org"
    //};
    //var service = scope.ServiceProvider.GetRequiredService<IDropBoxService>();
    //var jsonString = JsonConvert.SerializeObject(test, Formatting.Indented);
    //var fileName = $"SupportTicket_{Guid.NewGuid()}.json";
    //await service.UploadFileAsync(jsonString, fileName);
}

app.MapRazorPages();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapHub<DiscussionHub>("/discussionHub");
app.Run();