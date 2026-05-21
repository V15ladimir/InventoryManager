using System.Net;
using InventoryManager.Data;
using InventoryManager.Exceptions;
using InventoryManager.Models.Dto;
using InventoryManager.Services.Extensions;
using InventoryManager.Services.Mappers;
using InventoryManager.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Services {

    public class ItemService(ApplicationDbContext context) : IItemService {

        public async Task<InventoryItemsDto> GetItemsAsync(int inventoryId, PagedRequest pagedRequest) {
            var fields = await GetItemFieldsAsync(inventoryId);
            var itemValues = await GetItemValuesAsync(inventoryId, pagedRequest);
            return itemValues.ToDto(inventoryId, fields);
        }

        public async Task<List<InventoryItemValueDto>> GetItemValuesAsync(int itemId) {
            return await context.ItemValues.AsNoTracking()
                .Where(x => x.ItemId == itemId)
                .OrderBy(x => x.Field.Order)
                .Select(x => x.ToViewModel())
                .ToListAsync();
        }

        public async Task<List<InventoryFieldDto>> GetItemFieldsAsync(int inventoryId) {
            return await context.Fields.AsNoTracking()
                .Where(x => x.InventoryId == inventoryId)
                .OrderBy(x => x.Order)
                .Select(x => x.ToDto())
                .ToListAsync();
        }

        public async Task<InventoryItemValuesDto> GetItemAsync(int itemId) {
            var item = await context.Items
                .Include(x => x.ItemValues)
                .FirstOrDefaultAsync(x => x.Id == itemId) ??
                throw new NotFoundException("Item not found");
            return item.ToDto(item.ItemValues);
        }

        public async Task<string> GetItemCustomIdAsync(int inventoryId) {
            var nextSequence = await GenerateNextSequenceAsync();
            var idParts = await GenerateIdPartsAsync(inventoryId, nextSequence);
            var customId = idParts.Count > 0 ? string.Concat(idParts) : nextSequence.ToString();
            return customId;
        }

        public async Task CreateItemAsync(CreateInventoryItemDto item) {
            var nextSequence = await GenerateNextSequenceAsync();
            var itemEntity = item.ToEntity(nextSequence);
            itemEntity.SearchText = ItemSearchBuilder.Build(itemEntity);
            await context.Items.AddAsync(itemEntity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(UpdateInventoryItemDto itemDto) {
            var nextSequence = await GenerateNextSequenceAsync();
            var itemValues = await context.ItemValues.Where(x => x.ItemId == itemDto.ItemId)
                .ToListAsync();
            context.ItemValues.RemoveRange(itemValues);
            var item = await context.Items.FindAsync(itemDto.ItemId) ?? 
                throw new NotFoundException("Item not found");
            item.UpdateEntity(nextSequence, itemDto);
            await context.SaveChangesAsync();
        }

        public async Task DeleteItemsAsync(int inventoryId, List<int> selectedIds) {
            await context.Items.Where(x => selectedIds.Contains(x.Id))
                .ExecuteDeleteAsync();
        }

        private async Task<PagedList<InventoryItemValuesDto>> GetItemValuesAsync(int inventoryId, PagedRequest pagedRequest) {
            return await context.Items.AsNoTracking()
                .Where(x => x.InventoryId == inventoryId)
                .ApplyFilters(pagedRequest)
                .ApplySorting(pagedRequest)
                .Select(x => x.ToDto(x.ItemValues.OrderBy(iv => iv.Field.Order).ToList()))
                .ToPagedResponseAsync(pagedRequest);
        }

        private async Task<List<string>> GenerateIdPartsAsync(int inventoryId, int nextSequence) {
            return await context.IdParts.AsNoTracking()
               .Where(x => x.InventoryId == inventoryId)
               .OrderBy(x => x.Order)
               .Select(x => x.Build(nextSequence))
               .ToListAsync();
        }

        private async Task<int> GenerateNextSequenceAsync() {
            var sequence = await context.Items.AsNoTracking()
                .OrderByDescending(x => x.Sequence)
                .Select(x => x.Sequence)
                .FirstOrDefaultAsync();
            return sequence + 1;
        }
    }
}
