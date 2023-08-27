using Application.Analyzer.Commands.ParseDocument;
using FluentValidation;

namespace Application.Analyzer.Commands.RegisterDocument
{
    /// <summary>
    /// Validator for the ParseDocumentCommand.
    /// </summary>
    public class ParseDocumentCommandValidator : AbstractValidator<ParseDocumentCommand>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ParseDocumentCommandValidator()
        {
            RuleFor(x => x.DocumentId).NotNull();
        }
    }
}
