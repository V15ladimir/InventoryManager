using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites;
using InventoryManager.Models.ViewModels.Inventories.Shared;
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

        public static InventoryAccessListViewModel ToListViewModel(
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
                access.SearchText
            );
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
