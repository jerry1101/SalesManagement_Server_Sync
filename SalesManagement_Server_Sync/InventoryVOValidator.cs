using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement_Server_Sync.Inventory
{

    public class InventoryVOValidator : AbstractValidator<InventoryVO>
    {
        public InventoryVOValidator()
        {
            RuleFor(x => x.SKU)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("'SKU' should not be empty.")
                .WithErrorCode("ShouldNotBeEmpty");
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("'Quantity' should not be empty.")
                .WithErrorCode("ShouldNotBeEmpty");
        }
    }
}
