using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites;
using InventoryManager.Models.ViewModels.Users;
using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Services.Mappers {

    public static class UserMapper {

        public static UserDto ToDto(this ApplicationUser user, bool isAdmin, bool isBlock) {
            return new UserDto {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                IsAdmin = isAdmin,
                IsBlock = isBlock,
                CreatedAt = user.CreatedAt
            };
        }

        public static UsersIndexViewModel ToViewModel(this PagedList<UserDto> users) {
            return new UsersIndexViewModel {
                Users = new PagedList<UserViewModel>(
                    [.. users.Elements.Select(x => x.ToViewModel())],
                    users.PageIndex,
                    users.PageSize,
                    users.TotalCount,
                    users.SortBy,
                    users.SortOrder,
                    users.SearchText)
            };
        }

        private static UserViewModel ToViewModel(this UserDto user) {
            return new UserViewModel {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                IsBlocked = user.IsBlock,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
