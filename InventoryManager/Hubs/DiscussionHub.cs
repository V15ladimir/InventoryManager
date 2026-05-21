using Microsoft.AspNetCore.SignalR;

namespace InventoryManager.Hubs {

    public class DiscussionHub : Hub {

        public async Task JoinInventoryGroup(int inventoryId) {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"inventory-{inventoryId}");
        }

        public async Task LeaveInventoryGroup(int inventoryId) {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"inventory-{inventoryId}");
        }
    }
}
