using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites;
using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Models.ViewModels.Inventories.Form;
using InventoryManager.Models.ViewModels.Inventories.Index;
using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Services.Mappers {

    public static class InventoryMapper {

        public static InventoryDto ToDto(this Inventory inventory) {
            return new InventoryDto {
                InventoryId = inventory.Id,
                InventoryName = inventory.Name,
                InventoryCategoryId = inventory.CategoryId,
                IsPublicInventory = inventory.IsPublic,
                InventoryDescription = inventory.Description
            };
        }

        public static Inventory ToEntity(this CreateInventoryDto inventoryDto) {
            return new Inventory {
                Name = inventoryDto.InventoryName,
                CategoryId = inventoryDto.InventoryCategoryId,
                IsPublic = inventoryDto.IsPublicInventory,
                Description = inventoryDto.InventoryDescription,
                CreatedAt = inventoryDto.CreatedAt,
                CreatedById = inventoryDto.CreatedById
            };
        }

        public static InventoryViewModel ToViewModel(
            this Inventory inventory, 
            Category category,
            ApplicationUser? createdBy) => new() {
                Basic = new() {
                    Id = inventory.Id,
                    Name = inventory.Name,
                    Description = inventory.Description
                },
                Category = new() { CategoryName = category.Name },
                Status = new() { IsPublic = inventory.IsPublic },
                Audit = new() {
                    UpdatedAt = inventory.UpdatedAt,
                    CreatedAt = inventory.CreatedAt,
                    CreatedBy = createdBy?.UserName ?? string.Empty
                }
            };

        public static CreateInventoryDto ToCreateDto(
            this InventoryDetailsViewModel details,
            string? createdById) {
            return new CreateInventoryDto {
                InventoryName = details.InventoryName,
                InventoryCategoryId = details.InventoryCategoryId,
                IsPublicInventory = details.IsPublicInventory,
                InventoryDescription = details.InventoryDescription,
                CreatedById = createdById
            };
        }

        public static UpdateInventoryDto ToUpdateDto(
            this InventoryDetailsViewModel details,
            int inventoryId,
            string? updatedById) {
            return new UpdateInventoryDto {
                InventoryId = inventoryId,
                InventoryName = details.InventoryName,
                InventoryCategoryId = details.InventoryCategoryId,
                IsPublicInventory = details.IsPublicInventory,
                InventoryDescription = details.InventoryDescription,
                UpdatedById = updatedById
            };
        }

        public static InventorySettingsViewModel ToViewModel(this InventoryDto inventory,
            List<InventoryCategoryDto> categories,
            PagedRequest pagedRequest) {
            return new InventorySettingsViewModel {
                InventoryId = inventory.InventoryId,
                Details = inventory.ToDetailsViewModel(),
                Categories = categories.ToViewModel(),
                PagedRequest = pagedRequest
            };
        }

        private static InventoryDetailsViewModel ToDetailsViewModel(this InventoryDto inventory) {
            return new InventoryDetailsViewModel {
                InventoryName = inventory.InventoryName,
                InventoryCategoryId = inventory.InventoryCategoryId,
                IsPublicInventory = inventory.IsPublicInventory,
                InventoryDescription = inventory.InventoryDescription
            };
        }
    }
}
