using FluentValidation;

namespace Application.Analyzer.Commands.RegisterDocument
{
    /// <summary>
    /// Validator for the RegisterDocumentCommand.
    /// </summary>
    public class RegisterDocumentCommandValidator : AbstractValidator<RegisterDocumentCommand>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public RegisterDocumentCommandValidator()
        {
            RuleFor(x => x.FileLocation).NotEmpty();
        }
    }
}
