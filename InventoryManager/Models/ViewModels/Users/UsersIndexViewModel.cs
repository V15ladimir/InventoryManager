using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Models.ViewModels.Users {

    public class UsersIndexViewModel {
        public PagedList<UserViewModel> Users { get; set; } = new PagedList<UserViewModel>([]);
    }

    public class UserViewModel {
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
