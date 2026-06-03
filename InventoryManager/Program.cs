using FluentValidation;
using InventoryManager.Data;
using InventoryManager.Extensions;
using InventoryManager.Hubs;
using InventoryManager.Services;
using InventoryManager.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExternalAuthentication();
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddIntegrationServices(builder.Configuration);
builder.Services.AddControllersWithViews(options => {
    options.ModelValidatorProviders.Clear();
});
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IDiscussionService, DiscussionService>();
builder.Services.AddScoped<IAccessService, AccessService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddValidatorsFromAssemblyContaining<InventorySettingsValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<InventoryCustomIdPartsValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<InventoryCustomFieldsValidator>();

var app = builder.Build();
app.Use((context, next) => {
    context.Request.Scheme = "https";
    return next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

using(var scope = app.Services.CreateScope()) {
    await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>().InitializeAsync(scope.ServiceProvider);
}

app.MapRazorPages();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapHub<DiscussionHub>("/discussionHub");
app.Run();