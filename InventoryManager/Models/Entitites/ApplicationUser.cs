using Microsoft.AspNetCore.Identity;
using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Models.Entitites.Items;

namespace InventoryManager.Models.Entitites {

    public class ApplicationUser : IdentityUser {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LoginAt { get; set; }
        public ICollection<Inventory> CreatedInventories { get; set; } = [];
        public ICollection<Item> CreatedItems { get; set; } = [];
        public ICollection<ApplicationRole> Roles { get; set; } = [];
        public ICollection<InventoryAccess> AccessList { get; set; } = [];
    }
}
