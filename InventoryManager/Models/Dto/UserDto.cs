namespace InventoryManager.Models.Dto {

    public class UserDto {
        public string? UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public bool IsBlock { get; set; }
    }
}
