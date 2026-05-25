using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites.Custom;
using InventoryManager.Models.ViewModels.Inventories.Shared;

namespace InventoryManager.Services.Mappers {

    public static class InventoryFieldMapper {

        public static InventoryFieldDto ToDto(this Field field) {
            return new InventoryFieldDto() {
                Id = field.Id,
                Order = field.Order,
                Name = field.Name,
                Description = field.Description,
                FieldType = field.FieldType,
                FieldState = field.FieldState
            };
        }

        public static InventoryFieldViewModel ToViewModel(this Field field) {
            return new InventoryFieldViewModel() {
                Id = field.Id,
                Order = field.Order,
                Name = field.Name,
                Description = field.Description,
                Type = field.FieldType,
                State = field.FieldState
            };
        }

        public static InventoryCustomFieldsViewModel ToFieldsViewModel(
            this List<InventoryFieldDto> fields,
            InventoryDto inventory) {
            return new InventoryCustomFieldsViewModel {
                InventoryId = inventory.InventoryId,
                CustomFields = [.. fields.Select(x => x.ToViewModel())]
            };
        }

        public static InventoryFieldViewModel ToViewModel(this InventoryFieldDto field) {
            return new() {
                Id = field.Id,
                Order = field.Order,
                Name = field.Name,
                Description = field.Description,
                Type = field.FieldType,
                State = field.FieldState
            };
        }

        public static ItemFieldViewModel ToViewModel2(this InventoryFieldDto field) {
            return new ItemFieldViewModel {
                Id = field.Id,
                Type = field.FieldType,
                Name = field.Name
            };
        }
    }
}
