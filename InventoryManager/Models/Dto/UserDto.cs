namespace InventoryManager.Models.Dto {

    public class UserDto {
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBlock { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
