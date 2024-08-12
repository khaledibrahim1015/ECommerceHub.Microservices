using FluentValidation;
using MediatR;
using ValidationException = FluentValidation.ValidationException;
namespace Ordering.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            //This runs all the validation rules one by one returns the validation result
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            //Now, need to check for any failures
            var failures = validationResults.SelectMany(vr => vr.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
                throw new ValidationException(errors: failures);

        }
        return await next();


    }
}
