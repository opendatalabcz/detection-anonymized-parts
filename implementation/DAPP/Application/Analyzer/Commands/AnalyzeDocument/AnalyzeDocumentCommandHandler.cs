using Application.Analyzer.Commands.ParseDocument;
using Application.Analyzer.Commands.RegisterDocument;
using Domain.DocumentAggregate;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.AnalyzeDocument
{
    public class AnalyzeDocumentCommandHandler : IRequestHandler<AnalyzeDocumentCommand, ErrorOr<Document>>
    {
        private readonly IMediator mediator;

        public AnalyzeDocumentCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ErrorOr<Document>> Handle(AnalyzeDocumentCommand request, CancellationToken cancellationToken)
        {
            // Register the new document
            var registerDocumentCommand = new RegisterDocumentCommand { /* Initialize properties */ };
            var document = await mediator.Send(registerDocumentCommand);

            // Parse the PDF to jpg images
            var parsePdfCommand = new ParseDocumentCommand { /* Initialize properties */ };
            await mediator.Send(parsePdfCommand);

            return Error.NotFound();
        }
    }
}
