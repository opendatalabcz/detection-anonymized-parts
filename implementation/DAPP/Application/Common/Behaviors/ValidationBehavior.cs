using ErrorOr;

using FluentValidation;

using MediatR;

namespace Application.Common.Behaviors
{

    public sealed class ValidationBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        private readonly IValidator<TRequest>? validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            this.validator = validator;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (validator == null)
            {
                return await next();
            }

            // before the handler
            FluentValidation.Results.ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid)
            {
                return await next();
            }

            List<Error> errors = validationResult.Errors
                .ConvertAll(validationFailure => Error.Validation(
                     validationFailure.PropertyName,
                     validationFailure.ErrorMessage));

            return (dynamic)errors;
        }
    }
}