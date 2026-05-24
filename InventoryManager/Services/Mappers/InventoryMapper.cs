using System.Runtime.CompilerServices;
using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites;
using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Models.ViewModels.Inventories.Form;
using InventoryManager.Models.ViewModels.Inventories.Index;
using InventoryManager.Models.ViewModels.Inventories.Shared;
using InventoryManager.Models.ViewModels.Items.Index;
using InventoryManager.Services.Extensions;
using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Services.Mappers {

    public static class InventoryMapper {

        public static InventoryViewModel ToViewModel(
            this Inventory inventory, 
            Category category,
            ApplicationUser? createdBy) => new() {
            Basic = inventory.ToBasicViewModel(),
            Category = inventory.ToCategoryViewModel(category),
            Status = inventory.ToStatusViewModel(),
            Audit = inventory.ToAuditViewModel(createdBy)
        };

        public static InventoryBasicViewModel ToBasicViewModel(this Inventory inventory) => new() {
            Id = inventory.Id,
            Name = inventory.Name,
            Description = inventory.Description
        };

        public static InventoryAuditViewModel ToAuditViewModel(this Inventory inventory, ApplicationUser? createdBy) => new() {
            UpdatedAt = inventory.UpdatedAt,
            CreatedAt = inventory.CreatedAt,
            CreatedBy = createdBy?.UserName ?? string.Empty
        };

        public static InventoryCategViewModel ToCategoryViewModel(this Inventory inventory, Category category) => new() { 
            CategoryName = category.Name
        };

        public static InventoryStatusViewModel ToStatusViewModel(this Inventory inventory) => new() { 
            IsPublic = inventory.IsPublic
        };

        public static InventoryDto ToDto(this Inventory inventory) {
            return new InventoryDto {
                InventoryId = inventory.Id,
                InventoryName = inventory.Name,
                InventoryCategoryId = inventory.CategoryId,
                IsPublicInventory = inventory.IsPublic,
                InventoryDescription = inventory.Description
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

        private static InventorySettingsViewModel ToInventorySettingsViewModel(
            this InventoryDetailsViewModel details, 
            int inventoryId,
            List<InventoryCategoryDto> categories,
            PagedRequest pagedRequest) {
            return new InventorySettingsViewModel {
                InventoryId = inventoryId,
                Details = details,
                Categories = categories.ToViewModel(),
                PagedRequest = pagedRequest
            };
        }

        public static CreateInventoryDto ToCreateInventoryDto(
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

        public static UpdateInventoryDto ToUpdateInventoryDto(
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

        public static UpdateInventoryAccessDto ToUpdateInventoryAccessDto(
            this InventoryAccessViewModel access,
            int inventoryId) {
            return new UpdateInventoryAccessDto {
                InventoryId = inventoryId,
                UserId = access.UserId,
                HasAccess = access.HasAccess
            };
        }

        public static InventoryAccess CreateEntity(this UpdateInventoryAccessDto access) {
            return new InventoryAccess {
                InventoryId = access.InventoryId,
                UserId = access.UserId ?? string.Empty
            };
        }

        public static Inventory CreateEntity(this CreateInventoryDto inventoryDto) {
            return new Inventory {
                Name = inventoryDto.InventoryName,
                CategoryId = inventoryDto.InventoryCategoryId,
                IsPublic = inventoryDto.IsPublicInventory,
                Description = inventoryDto.InventoryDescription,
                CreatedAt = inventoryDto.CreatedAt,
                CreatedById = inventoryDto.CreatedById
            };
        }

        public static Inventory UpdateEntity(this UpdateInventoryDto inventoryDto, Inventory inventory) {
            inventory.Name = inventoryDto.InventoryName;
            inventory.CategoryId = inventoryDto.InventoryCategoryId;
            inventory.IsPublic = inventoryDto.IsPublicInventory;
            inventory.Description = inventoryDto.InventoryDescription;
            inventory.UpdatedAt = inventoryDto.UpdatedAt;
            return inventory;
        }

        public static InventorySettingsViewModel GetInventorySettingsViewModel(
            List<InventoryCategoryDto> categories,
            PagedRequest pagedRequest) {
            return new InventorySettingsViewModel {
                Details = new InventoryDetailsViewModel(),
                Categories = categories.ToViewModel(),
                PagedRequest = pagedRequest
            };
        }

        public static InventoryCustomIdPartsViewModel ToCustomIdPartsViewModel(
            this List<InventoryIdPartDto> parts, 
            InventoryDto inventory) {
            var inventoryIdParts = parts.Select(x => x.ToViewModel())
                .ToList();

            return new InventoryCustomIdPartsViewModel {
                InventoryId = inventory.InventoryId,
                CustomIdParts = inventoryIdParts
            };
        }

        public static InventoryCustomFieldsViewModel ToCustomIdFieldsViewModel(
            this List<InventoryFieldDto> fields,
            InventoryDto inventory) {
            var inventoryFields = fields.Select(x => x.ToViewModel())
                .ToList();

            return new InventoryCustomFieldsViewModel {
                InventoryId = inventory.InventoryId,
                CustomFields = inventoryFields
            };
        }

        public static ItemsIndexViewModel ToItemsIndexViewModel(
            this InventoryDto inventory, 
            bool hasSuperAccess,
            List<InventoryCategoryDto> categories,
            List<InventoryIdPartDto> parts,
            List<InventoryFieldDto> fields,
            PagedRequest pagedrequest) {
            var details = inventory.ToDetailsViewModel();
            return new ItemsIndexViewModel {
                InventoryId = inventory.InventoryId,
                Settings = details.ToInventorySettingsViewModel(inventory.InventoryId, categories, pagedrequest),
                Parts = parts.ToCustomIdPartsViewModel(inventory),
                Fields = fields.ToCustomIdFieldsViewModel(inventory),
                HasSuperAccess = hasSuperAccess
            };
        }
    }
}
