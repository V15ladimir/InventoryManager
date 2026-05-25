using InventoryManager.Data;
using InventoryManager.Exceptions;
using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites.Custom;
using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Models.ViewModels.Inventories;
using InventoryManager.Services.Extensions;
using InventoryManager.Services.Mappers;
using InventoryManager.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Services {

    public class InventoryService(ApplicationDbContext context) : IInventoryService {

        public async Task<List<InventoryViewModel>> GetLatestInventories() {
            return await context.Inventories.AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .Select(x => x.ToViewModel(x.Category, x.CreatedBy))
                .ToListAsync();
        }

        public async Task<List<InventoryViewModel>> GetTopInventories() {
            return await context.Inventories.AsNoTracking()
                .OrderByDescending(x => x.Items.Count())
                .Take(5)
                .Select(x => x.ToViewModel(x.Category, x.CreatedBy))
                .ToListAsync();
        }

        public async Task<PagedList<InventoryViewModel>> GetMyInventoriesAsync(string? userId, PagedRequest pagedRequest) {
            return await context.Inventories.AsNoTracking()
                .Where(x => x.CreatedById == userId)
                .ApplyFilters(pagedRequest)
                .ApplySorting(pagedRequest)
                .Select(x => x.ToViewModel(x.Category, x.CreatedBy))
                .ToPagedResponseAsync(pagedRequest);
        }

        public async Task<PagedList<InventoryViewModel>> GetSharedInventoriesAsync(string? userId, PagedRequest pagedRequest) {
            return await context.Inventories.AsNoTracking()
                .Where(x => x.AccessList.Any(x => x.UserId == userId))
                .ApplyFilters(pagedRequest)
                .ApplySorting(pagedRequest)
                .Select(x => x.ToViewModel(x.Category, x.CreatedBy))
                .ToPagedResponseAsync(pagedRequest);
        }

        public async Task<PagedList<InventoryViewModel>> GetInventoriesAsync(PagedRequest pagedRequest) {
            return await context.Inventories.AsNoTracking()
                .ApplyFilters(pagedRequest)
                .ApplySorting(pagedRequest)
                .Select(x => x.ToViewModel(x.Category, x.CreatedBy))
                .ToPagedResponseAsync(pagedRequest);
        }

        public async Task<PagedList<InventoryAccessDto>> GetInventoryAccessAsync(int inventoryId, PagedRequest pagedRequest) {
            var access = await GetAccessAsync(inventoryId);
            return await context.Users.AsNoTracking()
                .ApplyFilters(pagedRequest)
                .ApplySorting(pagedRequest, access)
                .Select(x => x.ToDto(access.ContainsKey(x.Id)))
                .ToPagedResponseAsync(pagedRequest);
        }

        public async Task<List<InventoryIdPartDto>> GetInventoryIdPartsAsync(int inventoryId) {
            return await context.IdParts.AsNoTracking()
                .Where(x => x.InventoryId == inventoryId)
                .OrderBy(x => x.Order)
                .Select(x => x.ToDto())
                .ToListAsync();
        }

        public async Task<List<InventoryFieldDto>> GetInventoryFieldsAsync(int inventoryId) {
            return await context.Fields.AsNoTracking()
                .Where(x => x.InventoryId == inventoryId)
                .OrderBy(x => x.Order)
                .Select(x => x.ToDto())
                .ToListAsync();
        }

        public async Task<InventoryDto> GetInventoryAsync(int inventoryId) {
            var inventory = await context.Inventories.FindAsync(inventoryId) ??
                throw new NotFoundException("Inventory not found");
            return inventory.ToDto();
        }

        public async Task<Inventory> CreateInventoryAsync(CreateInventoryDto inventoryDto) {
            var inventory = inventoryDto.ToEntity();
            await context.Inventories.AddAsync(inventory);
            await context.SaveChangesAsync();
            return inventory;
        }

        public async Task<Inventory> UpdateInventoryAsync(UpdateInventoryDto inventoryDto) {
            var inventory = await context.Inventories.FindAsync(inventoryDto.InventoryId) ??
                throw new NotFoundException("Inventory not found");
            inventory.Name = inventoryDto.InventoryName;
            inventory.CategoryId = inventoryDto.InventoryCategoryId;
            inventory.IsPublic = inventoryDto.IsPublicInventory;
            inventory.Description = inventoryDto.InventoryDescription;
            inventory.UpdatedAt = inventoryDto.UpdatedAt;
            await context.SaveChangesAsync();
            return inventory;
        }

        public async Task UpdateInventoryAccessAsync(UpdateInventoryAccessDto accessDto) {
            var _ = accessDto.HasAccess ? 
                context.InventoryAccess.Add(accessDto.ToEntity()) : 
                context.InventoryAccess.Remove(await GetAccessAsync(accessDto.InventoryId, accessDto.UserId));
            await context.SaveChangesAsync();
        }

        public async Task UpdateCustomIdPartsAsync(int inventoryId, InventoryCustomIdPartsViewModel parts) {
            var idParts = await GetIdPartsAsync(inventoryId);
            context.IdParts.RemoveRange(idParts);
            context.IdParts.AddRange(parts.CustomIdParts
                .Select(x => x.ToEntity(inventoryId))
                .ToList());
            await context.SaveChangesAsync();
        }

        public async Task<List<InventoryFieldDto>> UpdateCustomFieldsAsync(int inventoryId, InventoryCustomFieldsViewModel fieldsDto) {
            var fields = await GetFieldsAsync(inventoryId);
            var fieldIds = fields.ToDictionary(f => f.Id);
            var fieldsToCreate = new List<Field>();
            foreach(var field in fieldsDto.CustomFields) {
                if(fieldIds.TryGetValue(field.Id, out var existing)) {
                    existing.Name = field.Name;
                    existing.Description = field.Description;
                    existing.Order = field.Order;
                    existing.FieldState = field.State;
                    existing.FieldType = field.Type;
                    fieldIds.Remove(existing.Id);
                } else if(field.Id == 0) {
                    fieldsToCreate.Add(field.ToEntity(inventoryId));
                }
            }
            context.Fields.RemoveRange(fieldIds.Values);
            context.Fields.AddRange(fieldsToCreate);
            await context.SaveChangesAsync();
            var newFields = await GetFieldsAsync(inventoryId);
            return newFields.Select(x => x.ToDto()).ToList();
        }

        public async Task<int> DeleteInventoryAsync(List<int> inventoryIds) {
            return await context.Inventories.Where(x => inventoryIds.Contains(x.Id))
                .ExecuteDeleteAsync();
        }
        
        private async Task<List<IdPart>> GetIdPartsAsync(int inventoryId) {
            return await context.IdParts.Where(x => x.InventoryId == inventoryId)
                .ToListAsync();
        }

        private async Task<List<Field>> GetFieldsAsync(int inventoryId) {
            return await context.Fields.Where(x => x.InventoryId == inventoryId)
               .ToListAsync();
        }

        private async Task<Dictionary<string, InventoryAccess>> GetAccessAsync(int inventoryId) {
            return await context.InventoryAccess.Where(x => x.InventoryId == inventoryId)
                .ToDictionaryAsync(x => x.UserId);
        }

        private async Task<InventoryAccess> GetAccessAsync(int inventoryId, string? userId) {
            return await context.InventoryAccess.FirstOrDefaultAsync(x => x.InventoryId == inventoryId &&
                x.UserId == userId) ?? throw new NotFoundException("Access not found");
        }
    }
}
