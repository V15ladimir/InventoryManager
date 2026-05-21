using InventoryManager.Models.Entitites.Custom;
using InventoryManager.Models.Enums;
using InventoryManager.Models.ViewModels.Inventories.Shared;

namespace InventoryManager.Services.Extensions {

    public static class MappingFieldExtensions {

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

        private static T MapToField<T>(this InventoryFieldViewModel field, int inventoryId) where T: Field, new() => new() {
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
