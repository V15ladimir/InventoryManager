using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites.Custom;
using InventoryManager.Models.ViewModels.Inventories.Shared;

namespace InventoryManager.Services.Mappers {

    public static class InventoryFieldMapper {

        public static InventoryFieldViewModel ToView(this Field field) {
            return new() {
                Id = field.Id,
                Order = field.Order,
                Name = field.Name,
                Description = field.Description,
                Type = field.FieldType,
                State = field.FieldState
            };
        }

        public static InventoryFieldDto ToDto(this Field field) {
            return new() {
                Id = field.Id,
                Order = field.Order,
                Name = field.Name,
                Description = field.Description,
                Type = field.FieldType,
                State = field.FieldState
            };
        }

        public static void UpdateEntity(this Field field, InventoryFieldViewModel fieldDto) {
            field.Name = fieldDto.Name;
            field.Description = fieldDto.Description;
            field.Order = fieldDto.Order;
            field.FieldState = fieldDto.State;
            field.FieldType = fieldDto.Type;
        }

        public static InventoryFieldViewModel ToViewModel(this InventoryFieldDto field) {
            return new() {
                Id = field.Id,
                Order = field.Order,
                Name = field.Name,
                Description = field.Description,
                Type = field.Type,
                State = field.State
            };
        }
    }
}
