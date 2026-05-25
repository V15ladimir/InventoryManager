using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites;
using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Models.ViewModels.Inventories;
using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Services.Mappers {

    public static class InventoryAccessMapper {

        public static InventoryAccessDto ToDto(
            this ApplicationUser applicationUser, 
            bool hasAccess) {
            return new InventoryAccessDto {
                UserId = applicationUser.Id,
                UserName = applicationUser.UserName,
                Email = applicationUser.Email,
                CreatedAt = applicationUser.CreatedAt,
                HasAccess = hasAccess
            };
        }

        public static UpdateInventoryAccessDto ToUpdateDto(
            this InventoryAccessViewModel access,
            int inventoryId) {
            return new UpdateInventoryAccessDto {
                InventoryId = inventoryId,
                UserId = access.UserId,
                HasAccess = access.HasAccess
            };
        }

        public static InventoryAccess ToEntity(this UpdateInventoryAccessDto access) {
            return new InventoryAccess {
                InventoryId = access.InventoryId,
                UserId = access.UserId ?? string.Empty
            };
        }

        public static InventoryAccessListViewModel ToViewModel(
            this PagedList<InventoryAccessDto> access,
            int inventoryId) {
            return new InventoryAccessListViewModel {
                InventoryId = inventoryId,
                AccessList = access.ToPagedViewModel()
            };
        }

        private static PagedList<InventoryAccessViewModel> ToPagedViewModel(this PagedList<InventoryAccessDto> access) {
            return new PagedList<InventoryAccessViewModel>(
                [.. access.Elements.Select(x => x.ToViewModel())],
                access.PageIndex,
                access.PageSize,
                access.TotalCount,
                access.SortBy,
                access.SortOrder,
                access.SearchText);
        }

        private static InventoryAccessViewModel ToViewModel(this InventoryAccessDto access) {
            return new InventoryAccessViewModel {
                UserId = access.UserId,
                UserName = access.UserName,
                Email = access.Email,
                CreatedAt = access.CreatedAt,
                HasAccess = access.HasAccess
            };
        }
    }
}
