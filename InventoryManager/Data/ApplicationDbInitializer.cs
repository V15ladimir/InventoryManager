using InventoryManager.Exceptions;
using InventoryManager.Models.Entitites;
using InventoryManager.Models.Entitites.Custom;
using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Models.Entitites.Items;
using InventoryManager.Models.Enums;
using InventoryManager.Services.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Data {
    public class ApplicationDbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {

        public async Task InitializeAsync(IServiceProvider serviceProvider) {
            await context.Database.EnsureCreatedAsync();
            await SeedCategoriesAsync();
            await SeedUsersAsync();
            await SeedInventoriesAsync();
        }

        private async Task SeedCategoriesAsync() {
            if(context.Categories.Any())
                return;

            var categories = new[] {
                new Category { Name = "Equipment" },
                new Category { Name = "Furniture" },
                new Category { Name = "Book" },
                new Category { Name = "Software" },
                new Category { Name = "Other" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        private async Task SeedUsersAsync() {
            if(userManager.Users.Any())
                return;

            var roleExists = await roleManager.RoleExistsAsync("Admin");
            if(!roleExists) {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var admin = new ApplicationUser {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "User"
            };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");

            var user = new ApplicationUser {
                UserName = "user@example.com",
                Email = "user@example.com",
                EmailConfirmed = true,
                FirstName = "Regular",
                LastName = "User"
            };
            await userManager.CreateAsync(user, "User123!");

            var users = new[] {
                new ApplicationUser { UserName = "alice@example.com", Email = "alice@example.com", EmailConfirmed = true, FirstName = "Alice", LastName = "Smith" },
                new ApplicationUser { UserName = "bob@example.com", Email = "bob@example.com", EmailConfirmed = true, FirstName = "Bob", LastName = "Johnson" },
                new ApplicationUser { UserName = "charlie@example.com", Email = "charlie@example.com", EmailConfirmed = true, FirstName = "Charlie", LastName = "Brown" }
            };

            foreach(var testUser in users) {
                await userManager.CreateAsync(testUser, "Test123!");
            }
        }

        private async Task SeedInventoriesAsync() {
            if(context.Inventories.Any())
                return;

            var users = await userManager.Users.ToListAsync();
            var admin = users.First(u => u.Email == "admin@example.com");
            var regularUser = users.First(u => u.Email == "user@example.com");
            var equipmentCategory = await context.Categories.FirstAsync(c => c.Name == "Equipment");
            var bookCategory = await context.Categories.FirstAsync(c => c.Name == "Book");

            var equipmentInventory = new Inventory {
                Name = "Office Equipment",
                Description = "All office equipment and devices",
                CategoryId = equipmentCategory.Id,
                IsPublic = true,
                CreatedById = admin.Id,
                CreatedAt = DateTime.UtcNow,
                Elements = new List<IdPart> {
                    new FixedTextPart { Text = "EQ-", Order = 1 },
                    new SequencePart { Width = 4, Order = 2 },
                    new FixedTextPart { Text = "-", Order = 3 },
                    new DateTimePart { Format = "yyyy", Order = 4 }
                },
                Fields = new List<Field> {
                    new SinglelineField { Name = "Model", Description = "Device model", Length = 100, Order = 1 },
                    new SinglelineField { Name = "Serial Number", Length = 100, Order = 2 },
                    new NumberField { Name = "Price", Description = "Purchase price", MinValue = 0, MaxValue = 300, Order = 3 },
                    new SinglelineField { Name = "Location", Order = 4 },
                    new BooleanField { Name = "Is Working", Order = 5 },
                    new LinkField { Name = "Documentation", Order = 6 }
                }
            };

            var bookInventory = new Inventory {
                Name = "Library Books",
                Description = "Collection of library books",
                CategoryId = bookCategory.Id,
                IsPublic = true,
                CreatedById = regularUser.Id,
                CreatedAt = DateTime.UtcNow,
                Elements = new List<IdPart> {
                    new FixedTextPart { Text = "B", Order = 1 },
                    new Random6Part { Order = 2 },
                    new FixedTextPart { Text = "-", Order = 3 },
                    new SequencePart { Width = 3, Order = 4 }
                },
                Fields = new List<Field> {
                    new SinglelineField { Name = "Title", Order = 1 },
                    new SinglelineField { Name = "Author", Order = 2 },
                    new NumberField { Name = "Year", MinValue = 1800, MaxValue = 2025, Order = 3 },
                    new SinglelineField { Name = "ISBN", Order = 4 },
                    new BooleanField { Name = "Available", Order = 5 },
                    new MultilineField { Name = "Annotation", Order = 6 }
                }
            };

            var privateInventory = new Inventory {
                Name = "HR Documents (Private)",
                Description = "Sensitive HR documents - private access only",
                CategoryId = (await context.Categories.FirstAsync(c => c.Name == "Other")).Id,
                IsPublic = false,
                CreatedById = admin.Id,
                CreatedAt = DateTime.UtcNow,
                Elements = new List<IdPart> {
                    new FixedTextPart { Text = "HR-", Order = 1 },
                    new SequencePart { Width = 5, Order = 2 },
                    new FixedTextPart { Text = "/", Order = 3 },
                    new DateTimePart { Format = "MMyyyy", Order = 4 }
                },
                Fields = new List<Field> {
                    new SinglelineField { Name = "Document Name", FieldState = FieldState.NotPresent, Order = 1 },
                    new SinglelineField { Name = "Employee Name", Order = 2 },
                    new SinglelineField { Name = "Position", Order = 3 },
                    new LinkField { Name = "File Link", FieldState = FieldState.Optional, Order = 5 },
                    new BooleanField { Name = "Approved", Order = 6 }
                }
            };

            await context.Inventories.AddRangeAsync(equipmentInventory, bookInventory, privateInventory);
            await context.SaveChangesAsync();

            await SeedItemsForEquipmentInventory(equipmentInventory.Id, admin.Id);
            await SeedItemsForBookInventory(bookInventory.Id, regularUser.Id);
            await SeedItemsForPrivateInventory(privateInventory.Id, admin.Id);
        }

        private async Task SeedItemsForEquipmentInventory(int inventoryId, string createdBy) {
            var items = new[] {
                new { Model = "Dell XPS 15", Serial = "ABC123456", Price = 1499.99m, Location = "Office A", IsWorking = true },
                new { Model = "HP LaserJet Pro", Serial = "XYZ789012", Price = 499.99m, Location = "Office B", IsWorking = true },
                new { Model = "Logitech MX Master", Serial = "LGH987654", Price = 89.99m, Location = "Office A", IsWorking = false }
            };

            var inventory = await context.Inventories.FirstOrDefaultAsync(x => x.Id == inventoryId) ??
                throw new NotFoundException("Inventory not found");
            var values = inventory.Fields.Select((x,y) => new ItemValue {
                FieldId = x.Id,
                Value = $"{y} значение"
            }).ToList();

            var sequence = 0;
            foreach(var itemData in items) {
                sequence++;
                var customId = $"EQ-{sequence:D4}-{DateTime.UtcNow:yyyy}";
                var item = new Item {
                    InventoryId = inventoryId,
                    CustomId = customId,
                    Sequence = sequence,
                    CreatedById = createdBy,
                    CreatedAt = DateTime.UtcNow,
                    ItemValues = values
                };
                item.SearchText = ItemSearchBuilder.Build(item);
                await context.Items.AddAsync(item);
            }

            await context.SaveChangesAsync();
        }

        private async Task SeedItemsForBookInventory(int inventoryId, string createdBy) {
            var items = new[] {
                new { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Year = 1925, ISBN = "978-0-7432-7356-5", Available = true },
                new { Title = "To Kill a Mockingbird", Author = "Harper Lee", Year = 1960, ISBN = "978-0-06-112008-4", Available = true },
                new { Title = "1984", Author = "George Orwell", Year = 1949, ISBN = "978-0-452-28423-4", Available = false }
            };

            var inventory = await context.Inventories.FirstOrDefaultAsync(x => x.Id == inventoryId) ?? 
                throw new NotFoundException("Inventory not found");
            var values = inventory.Fields.Select((x, y) => new ItemValue {
                FieldId = x.Id,
                Value = $"{y} значение"
            }).ToList();

            var sequence = 0;
            var random = new Random();

            foreach(var itemData in items) {
                sequence++;
                var randomNum = random.Next(0, 999999).ToString("D6");
                var customId = $"B{randomNum}-{sequence:D3}";

                var item = new Item {
                    InventoryId = inventoryId,
                    CustomId = customId,
                    Sequence = sequence,
                    CreatedById = createdBy,
                    CreatedAt = DateTime.UtcNow,
                    ItemValues = values
                };
                item.SearchText = ItemSearchBuilder.Build(item);
                await context.Items.AddAsync(item);
            }

            await context.SaveChangesAsync();
        }

        private async Task SeedItemsForPrivateInventory(int inventoryId, string createdBy) {
            var items = new[] {
                new { Name = "Employment Contract", Employee = "John Doe", Position = "Software Engineer", Approved = true },
                new { Name = "NDA Agreement", Employee = "Jane Smith", Position = "Product Manager", Approved = true },
                new { Name = "Performance Review", Employee = "Bob Wilson", Position = "Senior Developer", Approved = false }
            };

            var inventory = await context.Inventories.FirstOrDefaultAsync(x => x.Id == inventoryId) ??
                throw new NotFoundException("Inventory not found");
            var values = inventory.Fields.Select((x, y) => new ItemValue {
                FieldId = x.Id,
                Value = $"{y} значение"
            }).ToList();

            var sequence = 0;
            foreach(var itemData in items) {
                sequence++;
                var customId = $"HR-{sequence:D5}/{DateTime.UtcNow:MMyyyy}";

                var item = new Item {
                    InventoryId = inventoryId,
                    CustomId = customId,
                    Sequence = sequence,
                    CreatedById = createdBy,
                    CreatedAt = DateTime.UtcNow,
                    ItemValues = values
                };

                item.SearchText = ItemSearchBuilder.Build(item);
                await context.Items.AddAsync(item);
            }

            await context.SaveChangesAsync();
        }
    }
}
