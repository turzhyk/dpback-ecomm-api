using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using FluentValidation;

namespace DPBack.Application.Validators.Orders;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator(IOrdersRepository repo)
    {
        RuleFor(x => x.Items).NotEmpty().WithMessage("Items are required");
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Item count must be greater than 0");
        });
        RuleFor(x => x.CustomerId)
            .MustAsync(async (id, cToken) => await repo.CustomerExistsAsync(id!.Value, cToken))
            .WithMessage("Customer not found")
            .When(x => x.CustomerId.HasValue);

        RuleFor(x => x.Paid).NotEmpty().WithMessage("Paid info is required");
        RuleFor(x => x.Desc).MaximumLength(200).WithMessage("Description should not exceed 200 characters");
        RuleFor(x => x.CreatedBy).NotEmpty().WithMessage("Author ID is required");
    }
}