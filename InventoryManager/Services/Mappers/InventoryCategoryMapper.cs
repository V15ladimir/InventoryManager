using System.Reflection.Metadata.Ecma335;
using InventoryManager.Models.Dto;
using InventoryManager.Models.ViewModels.Inventories.Form;

namespace InventoryManager.Services.Mappers {

    public static class InventoryCategoryMapper {

        public static InventoryCategoryViewModel ToViewModel(this InventoryCategoryDto category) {
            return new InventoryCategoryViewModel {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        public static List<InventoryCategoryViewModel> ToViewModel(this List<InventoryCategoryDto> categories) {
            return [.. categories.Select(x => x.ToViewModel())];
        }
    }
}
