using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites.Custom;
using InventoryManager.Models.Enums;
using InventoryManager.Models.ViewModels.Inventories;

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
            int inventoryId) {
            return new InventoryCustomFieldsViewModel {
                InventoryId = inventoryId,
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

        public static Field ToEntity(this InventoryFieldViewModel field, int inventoryId) {
            return field.Type switch {
                FieldType.SingleLine => field.MapToField<SinglelineField>(inventoryId),
                FieldType.MultiLine => field.MapToField<MultilineField>(inventoryId),
                FieldType.Number => field.MapToField<NumberField>(inventoryId),
                FieldType.Link => field.MapToField<LinkField>(inventoryId),
                FieldType.Boolean => field.MapToField<BooleanField>(inventoryId),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static T MapToField<T>(this InventoryFieldViewModel field, int inventoryId) where T : Field, new() => new() {
            Id = field.Id,
            InventoryId = inventoryId,
            Order = field.Order,
            Name = field.Name,
            Description = field.Description,
            FieldType = field.Type,
            FieldState = field.State
        };
    }
}
