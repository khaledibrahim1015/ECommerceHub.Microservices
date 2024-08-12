using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(o => o.UserName)
              .NotEmpty()
              .NotNull()
              .WithMessage("{UserName} is Required")
              .MaximumLength(100)
              .WithMessage("{UserName} must not exceed 70 characters");


        RuleFor(o => o.TotalPrice)
            .NotNull()
            .NotEmpty()
            .WithMessage("{TotalPrice} is required")
            .GreaterThan(-1)
            .WithMessage("{TotalPrice} should not be negative ");


        RuleFor(o => o.EmailAddress)
            .NotEmpty()
            .NotNull()
            .WithMessage("{EmailAddress} is required");


        RuleFor(o => o.FirstName)
        .NotEmpty()
        .NotNull()
        .WithMessage("{FirstName} is required");

        RuleFor(o => o.LastName)
            .NotEmpty()
            .NotNull()
            .WithMessage("{LastName} is required");
    }
}
