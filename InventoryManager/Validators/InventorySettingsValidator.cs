using FluentValidation;
using InventoryManager.Models.ViewModels.Inventories;

namespace InventoryManager.Validators {

    public class InventorySettingsValidator : AbstractValidator<InventorySettingsViewModel> {
        public InventorySettingsValidator() {
            RuleFor(x => x.Details.InventoryName)
                .NotEmpty()
                .WithMessage("Name is required");
        }
    }
}
