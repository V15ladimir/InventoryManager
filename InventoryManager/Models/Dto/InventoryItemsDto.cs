using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Models.Dto {

    public class InventoryItemsDto {
        public int InventoryId { get; set; }
        public List<InventoryFieldDto> Fields { get; set; } = [];
        public PagedList<InventoryItemValuesDto> Items = new([]);
    }
}
