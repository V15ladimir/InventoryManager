using InventoryManager.Models.Dto;
using InventoryManager.Models.Entitites;
using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Models.ViewModels.Discussions;
using InventoryManager.Utilities.Pagination;

namespace InventoryManager.Services.Mappers {

    public static class InventoryDiscussionMapper {

        public static InventoryDiscussionDto ToDto(this InventoryDiscussion discussion, ApplicationUser createdBy) {
            return new InventoryDiscussionDto {
                DiscussionId = discussion.Id,
                DiscussionContent = discussion.Content,
                AuthorId = createdBy.Id,
                AuthorName = createdBy.UserName ?? string.Empty,
                CreatedAt = discussion.CreatedAt
            };
        }

        public static CreateDiscussionDto ToDto(this CreateInventoryDicussionViewModel discussion, string? userId) {
            return new CreateDiscussionDto {
                InventoryId = discussion.InventoryId,
                Content = discussion.Content,
                UserId = userId
            };
        }

        public static InventoryDiscussion ToEntity(this CreateDiscussionDto discussion) {
            return new InventoryDiscussion {
                InventoryId = discussion.InventoryId,
                Content = discussion.Content,
                CreatedById = discussion.UserId ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static InventoryDiscussionListViewModel ToViewModel(this PagedList<InventoryDiscussionDto> discussions) {
            return new InventoryDiscussionListViewModel {
                Items = [.. discussions.Elements.Select(ToViewModel)],
                TotalCount = discussions.TotalCount,
                CurrentPage = discussions.PageIndex,
                PageSize = discussions.PageSize,
                TotalPages = discussions.TotalPages,
                HasMore = discussions.HasNextPage
            };
        }

        public static InventoryDiscussionViewModel ToViewModel(this InventoryDiscussionDto discussion) {
            return new InventoryDiscussionViewModel {
                Basic = discussion.ToBasicViewModel(),
                Author = discussion.ToAuthorViewModel(),
                Audit = discussion.ToAuditViewModel()
            };
        }

        private static InventoryDiscussionBasicViewModel ToBasicViewModel(this InventoryDiscussionDto discussion) {
            return new InventoryDiscussionBasicViewModel {
                Id = discussion.DiscussionId,
                Content = discussion.DiscussionContent
            };
        }

        private static InventoryDiscussionAuthorViewModel ToAuthorViewModel(this InventoryDiscussionDto discussion) {
            return new InventoryDiscussionAuthorViewModel {
                AuthorId = discussion.AuthorId,
                AuthorName = discussion.AuthorName
            };
        }

        private static InventoryDiscussionAuditViewModel ToAuditViewModel(this InventoryDiscussionDto discussion) {
            return new InventoryDiscussionAuditViewModel {
                CreatedAt = discussion.CreatedAt
            };
        }
    }
}
