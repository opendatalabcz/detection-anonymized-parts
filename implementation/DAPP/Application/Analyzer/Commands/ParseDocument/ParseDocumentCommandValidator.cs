using Application.Analyzer.Commands.ParseDocument;
using FluentValidation;

namespace Application.Analyzer.Commands.RegisterDocument
{
    public class ParseDocumentCommandValidator : AbstractValidator<ParseDocumentCommand>
    {
        public ParseDocumentCommandValidator()
        {
            RuleFor(x => x.DocumentId).NotNull();
        }
    }
}
