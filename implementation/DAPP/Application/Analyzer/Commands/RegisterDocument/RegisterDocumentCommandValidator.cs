using FluentValidation;

namespace Application.Analyzer.Commands.RegisterDocument
{
    public class RegisterDocumentCommandValidator : AbstractValidator<RegisterDocumentCommand>
    {
        public RegisterDocumentCommandValidator()
        {
            RuleFor(x => x.FileLocation).NotEmpty();
        }
    }
}
