using InventoryManager.Models.Dto;
using InventoryManager.Models.ViewModels.Inventories.Form;
using InventoryManager.Models.ViewModels.Inventories.Shared;

namespace InventoryManager.Services.Mappers {

    public static class InventoryPartMapper {
        public static InventoryCustomIdPartsViewModel ToViewModel(
            this List<InventoryIdPartDto> parts,
            InventoryDto inventory) {
            return new InventoryCustomIdPartsViewModel {
                InventoryId = inventory.InventoryId,
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
    }
}
