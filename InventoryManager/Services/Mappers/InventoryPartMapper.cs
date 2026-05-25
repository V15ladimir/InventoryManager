using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites.Custom;
using InventoryManager.Models.Enums;
using InventoryManager.Models.ViewModels.Inventories;

namespace InventoryManager.Services.Mappers {

    public static class InventoryPartMapper {
        public static InventoryCustomIdPartsViewModel ToViewModel(
            this List<InventoryIdPartDto> parts,
            int inventoryId) {
            return new InventoryCustomIdPartsViewModel {
                InventoryId = inventoryId,
                CustomIdParts = [.. parts.Select(x => x.ToViewModel())]
            };
        }

        public static InventoryIdPartViewModel ToViewModel(this InventoryIdPartDto inventory) {
            return new InventoryIdPartViewModel {
                Id = inventory.Id,
                Order = inventory.Order,
                Text = inventory.Text,
                Format = inventory.Format,
                NumberWidth = inventory.NumberWidth,
                UseHex = inventory.UseHex,
                Type = inventory.Type
            };
        }

        public static IdPart ToEntity(this InventoryIdPartViewModel idPart, int inventoryId) {
            return idPart.Type switch {
                IdType.FixedText => idPart.ToFixedTextEntity(inventoryId),
                IdType.Random20bit => idPart.MapToRandom20bitPart(inventoryId),
                IdType.Random32bit => idPart.MapToRandom32bitPart(inventoryId),
                IdType.Random6 => idPart.MapToRandom6Part(inventoryId),
                IdType.Random9 => idPart.MapToRandom9Part(inventoryId),
                IdType.Guid => idPart.MapToGuidPart(inventoryId),
                IdType.DateTime => idPart.MapToDateTimePart(inventoryId),
                IdType.Sequence => idPart.MapToSequence(inventoryId),
                _ => throw new ArgumentOutOfRangeException($"Unknow type {idPart.Type}")
            };
        }

        public static InventoryIdPartDto ToDto(this IdPart idPart) {
            return idPart switch {
                FixedTextPart part => part.ToDto(),
                Random20bitPart part => part.ToDto(),
                Random32bitPart part => part.ToDto(),
                Random6Part part => part.ToDto(),
                Random9Part part => part.ToDto(),
                GuidPart part => part.ToDto(),
                DateTimePart part => part.ToDto(),
                SequencePart part => part.ToDto(),
                _ => throw new ArgumentOutOfRangeException($"Unknow type {idPart.GetType}")
            };
        }

        private static FixedTextPart ToFixedTextEntity(this InventoryIdPartViewModel response, int inventoryId) => new() {
            InventoryId = inventoryId,
            Text = response.Text ?? string.Empty,
            Order = response.Order
        };

        private static Random20bitPart MapToRandom20bitPart(this InventoryIdPartViewModel response, int inventoryId) => new() {
            InventoryId = inventoryId,
            UseHex = response.UseHex ?? false,
            Width = response.NumberWidth ?? 5,
            Order = response.Order
        };

        private static Random32bitPart MapToRandom32bitPart(this InventoryIdPartViewModel response, int inventoryId) => new() {
            InventoryId = inventoryId,
            UseHex = response.UseHex ?? false,
            Width = response.NumberWidth ?? 8,
            Order = response.Order
        };

        private static Random6Part MapToRandom6Part(this InventoryIdPartViewModel response, int inventoryId) => new() {
            InventoryId = inventoryId,
            Width = response.NumberWidth ?? 6,
            Order = response.Order
        };

        private static Random9Part MapToRandom9Part(this InventoryIdPartViewModel response, int inventoryId) => new() {
            InventoryId = inventoryId,
            Width = response.NumberWidth ?? 9,
            Order = response.Order
        };

        private static GuidPart MapToGuidPart(this InventoryIdPartViewModel response, int inventoryId) => new() {
            InventoryId = inventoryId,
            Format = response.Format ?? "N",
            Order = response.Order
        };

        private static DateTimePart MapToDateTimePart(this InventoryIdPartViewModel response, int inventoryId) => new() {
            InventoryId = inventoryId,
            Format = response.Format ?? "yyyyMMddHHmmss",
            Order = response.Order
        };

        private static SequencePart MapToSequence(this InventoryIdPartViewModel response, int inventoryId) => new() {
            InventoryId = inventoryId,
            Width = response.NumberWidth ?? 0,
            Order = response.Order
        };

        private static InventoryIdPartDto ToDto(this FixedTextPart part) => new() {
            Id = part.Id,
            Order = part.Order,
            Text = part.Text,
            Format = null,
            NumberWidth = null,
            UseHex = null,
            Type = part.IdType
        };

        private static InventoryIdPartDto ToDto(this Random20bitPart part) => new() {
            Id = part.Id,
            Order = part.Order,
            Text = null,
            Format = null,
            NumberWidth = part.Width,
            UseHex = part.UseHex,
            Type = part.IdType
        };

        private static InventoryIdPartDto ToDto(this Random32bitPart part) => new() {
            Id = part.Id,
            Order = part.Order,
            Text = null,
            Format = null,
            NumberWidth = part.Width,
            UseHex = part.UseHex,
            Type = part.IdType
        };

        private static InventoryIdPartDto ToDto(this Random6Part part) => new() {
            Id = part.Id,
            Order = part.Order,
            Text = null,
            Format = null,
            NumberWidth = part.Width,
            UseHex = false,
            Type = part.IdType
        };

        private static InventoryIdPartDto ToDto(this Random9Part part) => new() {
            Id = part.Id,
            Order = part.Order,
            Text = null,
            Format = null,
            NumberWidth = part.Width,
            UseHex = false,
            Type = part.IdType
        };

        private static InventoryIdPartDto ToDto(this GuidPart part) => new() {
            Id = part.Id,
            Order = part.Order,
            Text = null,
            Format = part.Format,
            NumberWidth = null,
            UseHex = false,
            Type = part.IdType
        };

        private static InventoryIdPartDto ToDto(this DateTimePart part) => new() {
            Id = part.Id,
            Order = part.Order,
            Text = null,
            Format = part.Format,
            NumberWidth = null,
            UseHex = false,
            Type = part.IdType
        };

        private static InventoryIdPartDto ToDto(this SequencePart part) => new() {
            Id = part.Id,
            Order = part.Order,
            Text = null,
            Format = null,
            NumberWidth = part.Width,
            UseHex = false,
            Type = part.IdType
        };
    }
}
