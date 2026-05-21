using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites.Custom;
using InventoryManager.Models.Entitites.Items;
using InventoryManager.Models.ViewModels.Inventories.Shared;
using InventoryManager.Models.ViewModels.Items.Form;
using InventoryManager.Models.ViewModels.Items.Index;
using InventoryManager.Utilities.Pagination;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace InventoryManager.Services.Mappers {

    public static class InventoryItemMapper {

        public static InventoryItemsDto ToDto(
            this PagedList<InventoryItemValuesDto> items,
            int inventoryId,
            List<InventoryFieldDto> fields) {
            return new InventoryItemsDto {
                InventoryId = inventoryId,
                Fields = fields,
                Items = items
            };
        }

        public static InventoryItemValuesDto ToDto(
            this Item item,
            IEnumerable<ItemValue> itemValues) {
            return new InventoryItemValuesDto {
                ItemId = item.Id,
                InventoryId = item.InventoryId,
                CustomId = item.CustomId,
                ItemValues = [.. itemValues.Select(y => new InventoryItemValueDto { 
                    Id = y.Id, 
                    FieldId = y.FieldId, 
                    Value = y.Value 
                })]
            };
        }

        public static InventoryItemsViewModel ToViewModel(this InventoryItemsDto items) {
            return new InventoryItemsViewModel {
                InventoryId = items.InventoryId,
                Fields = [.. items.Fields.Select(x => x.ToViewModel2())],
                Items = new PagedList<ItemFieldValuesViewModel>(
                    [.. items.Items.Elements.Select(x => x.ToViewModel())],
                    items.Items.PageIndex,
                    items.Items.PageSize,
                    items.Items.TotalCount,
                    items.Items.SortBy,
                    items.Items.SortOrder,
                    items.Items.SearchText
                )
            };
        }

        private static ItemFieldViewModel ToViewModel2(this InventoryFieldDto field) {
            return new ItemFieldViewModel {
                Id = field.Id,
                Name = field.Name
            };
        }

        private static ItemFieldValuesViewModel ToViewModel(this InventoryItemValuesDto itemValues) {
            return new ItemFieldValuesViewModel {
                Id = itemValues.ItemId,
                CustomId = itemValues.CustomId,
                ItemValues = itemValues.ItemValues.Select(x => new ItemFieldValueViewModel { Id = x.Id, Value = x.Value })
            };
        }

        public static ItemFieldValuesViewModel ToView(
            this Item item, 
            ICollection<ItemValue> itemValues) {
            return new ItemFieldValuesViewModel {
                Id = item.Id,
                CustomId = item.CustomId,
                ItemValues = itemValues.Select(y => new ItemFieldValueViewModel{ Id = y.Id, Value = y.Value })
            };
        }

        public static ItemFormViewModel ToFormView(
            this Item item, 
            List<Field> fields,
            List<ItemValue> itemValues,
            PagedRequest pagedRequest) {
            return new ItemFormViewModel {
                Id = item.Id,
                InventoryId = item.InventoryId,
                CustomId = item.CustomId,
                Fields = fields.Select(x => x.ToView()).ToList(),
                FieldValues = itemValues.ToDictionary(x => x.FieldId, x => x.Value),
                PagedRequest = pagedRequest
            };
        }

        public static ItemFormViewModel ToViewModel(
            this List<InventoryFieldDto> fields,
            int itemId,
            int inventoryId,
            string? customId,
            PagedRequest pagedRequest,
            List<InventoryItemValueDto> itemValues) {
            return new ItemFormViewModel {
                Id = itemId,
                InventoryId = inventoryId,
                CustomId = customId ?? string.Empty,
                Fields = [.. fields.Select(x => x.ToViewModel())],
                FieldValues = itemValues.ToDictionary(x => x.FieldId, x => x.Value),
                PagedRequest = pagedRequest
            };
        }

        public static InventoryItemValueDto ToViewModel(this ItemValue itemValue) {
            return new InventoryItemValueDto {
                Id = itemValue.Id,
                FieldId = itemValue.FieldId,
                Value = itemValue.Value
            };
        }

        public static CreateInventoryItemDto ToDto(this ItemFormViewModel item) {
            return new CreateInventoryItemDto {
                InventoryId = item.InventoryId,
                CustomId = item.CustomId,
                FieldValues = item.FieldValues
            };
        }

        public static UpdateInventoryItemDto ToUpdateDto(this ItemFormViewModel item) {
            return new UpdateInventoryItemDto {
                ItemId = item.Id,
                CustomId = item.CustomId,
                FieldValues = item.FieldValues
            };
        }

        public static Item ToEntity(this CreateInventoryItemDto item, int sequence) {
            return new Item {
                InventoryId = item.InventoryId,
                CustomId = item.CustomId ?? sequence.ToString(),
                Sequence = sequence,
                ItemValues = [.. item.FieldValues.Select(x => new ItemValue {
                    FieldId = x.Key,
                    Value = x.Value
                })]
            };
        }

        public static InventoryItemViewModel ToViewModel(this ItemFormViewModel item) {
            return new InventoryItemViewModel {
                InventoryId = item.InventoryId,
                SortBy = item.PagedRequest.SortBy,
                SortOrder = item.PagedRequest.SortOrder,
                Page = item.PagedRequest.Page,
                PageSize = item.PagedRequest.PageSize,
                SearchText = item.PagedRequest.SearchText
            };
        }

        public static Item UpdateEntity(this Item item, int sequence, UpdateInventoryItemDto itemDto) {
            item.CustomId = itemDto.CustomId ?? sequence.ToString();
            item.ItemValues = [.. itemDto.FieldValues.Select(x => new ItemValue {
                FieldId = x.Key,
                Value = x.Value
            })];
            return item;
        }
    }
}
