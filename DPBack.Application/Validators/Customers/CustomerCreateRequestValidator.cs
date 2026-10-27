using DPBack.Application.Contracts.Customers;
using FluentValidation;

namespace DPBack.Application.Validators.Customers;

public class CustomerCreateRequestValidator:AbstractValidator<CustomerCreateRequest>
{
    public CustomerCreateRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().MaximumLength(50);
        RuleFor(x=> x.Phone).NotEmpty().WithMessage("Phone is required").MaximumLength(20);
    }
}