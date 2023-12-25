using System.Text;
using Application.Common.Errors;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Common.Behaviors;

public sealed class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IHttpResult, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        ValidationContext<object> context = new(request);

        // List<ValidationFailure> failures = _validators
        //     .Select(validator => validator.Validate(context))
        //     .SelectMany(result => result.Errors)
        //     .Where(failure => failure != null)
        //     .ToList();

        StringBuilder sb = new();

        _validators.Select(validator => validator.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .ToList()
            .ForEach(failure => sb.AppendLine(failure.ErrorMessage));


        string validationErrorMessage = sb.ToString();

        if (validationErrorMessage.Length > 0)
        {
            var response =
                new TResponse().CreateWith(ValidationErrors.InvalidCredentials(validationErrorMessage),
                    StatusCodes.Status400BadRequest);

            return Task.FromResult((TResponse)response);
        }

        return next();
    }
}