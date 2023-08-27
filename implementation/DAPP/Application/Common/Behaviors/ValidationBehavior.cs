using ErrorOr;

using FluentValidation;

using MediatR;

namespace Application.Common.Behaviors
{

    /// <summary>
    /// Validation behavior for the MediatR pipeline.
    /// </summary>
    /// <typeparam name="TRequest"> The request type.</typeparam>
    /// <typeparam name="TResponse"> The response type.</typeparam>
    public sealed class ValidationBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        private readonly IValidator<TRequest>? validator;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="validator"></param>
        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="next"> The next handler.</param>
        /// <param name="cancellationToken"> The cancellation token.</param>
        /// <returns> The response.</returns>
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