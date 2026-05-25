using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites.Custom;
using InventoryManager.Models.Enums;
using InventoryManager.Models.ViewModels.Inventories.Form;

namespace InventoryManager.Services.Extensions {

    public static class MappingIdPartExtensions {

        public static IdPart ToEntity(this InventoryIdPartViewModel idPart, int inventoryId) {
            return idPart.Type switch {
                IdType.FixedText => idPart.MapToFixedText(inventoryId),
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
                FixedTextPart part => new InventoryIdPartDto { 
                    Id = idPart.Id,
                    Order = part.Order, 
                    Text = part.Text, 
                    Format = null,
                    NumberWidth = null,
                    UseHex = null, 
                    Type = part.IdType
                },
                Random20bitPart part => new InventoryIdPartDto {
                    Id = idPart.Id,
                    Order = part.Order,
                    Text = null,
                    Format = null,
                    NumberWidth = part.Width,
                    UseHex = part.UseHex,
                    Type = part.IdType 
                },
                Random32bitPart part => new InventoryIdPartDto {
                    Id = idPart.Id,
                    Order = part.Order,
                    Text = null,
                    Format = null,
                    NumberWidth = part.Width,
                    UseHex = part.UseHex,
                    Type = part.IdType
                },
                Random6Part part => new InventoryIdPartDto {
                    Id = idPart.Id,
                    Order = part.Order,
                    Text = null,
                    Format = null,
                    NumberWidth = part.Width,
                    UseHex = false,
                    Type = part.IdType
                },
                Random9Part part => new InventoryIdPartDto {
                    Id = idPart.Id,
                    Order = part.Order,
                    Text = null,
                    Format = null,
                    NumberWidth = part.Width,
                    UseHex = false,
                    Type = part.IdType
                },
                GuidPart part => new InventoryIdPartDto {
                    Id = idPart.Id,
                    Order = part.Order,
                    Text = null,
                    Format = part.Format,
                    NumberWidth = null,
                    UseHex = false,
                    Type = part.IdType 
                },
                DateTimePart part => new InventoryIdPartDto {
                    Id = idPart.Id,
                    Order = part.Order,
                    Text = null,
                    Format = part.Format,
                    NumberWidth = null,
                    UseHex = false,
                    Type = part.IdType
                },
                SequencePart part => new InventoryIdPartDto {
                    Id = idPart.Id,
                    Order = part.Order,
                    Text = null,
                    Format = null,
                    NumberWidth = part.Width,
                    UseHex = false,
                    Type = part.IdType
                },
                _ => throw new ArgumentOutOfRangeException($"Unknow type {idPart.GetType}")
            };
        }

        

        private static FixedTextPart MapToFixedText(this InventoryIdPartViewModel response, int inventoryId) => new() {
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
    }
}
