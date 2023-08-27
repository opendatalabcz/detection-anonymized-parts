using Application.Common.Interfaces.Persistance;
using DAPPAnalyzer.Models;
using Domain.DocumentAggregate.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.ParseDocument
{
    /// <summary>
    /// Command Handler to parse a document.
    /// </summary>
    public class ParseDocumentCommandHandler : IRequestHandler<ParseDocumentCommand, ErrorOr<(DappPDF, DocumentId)>>
    {
        private readonly IDocumentRepository documentRepository;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="documentRepository"></param>
        public ParseDocumentCommandHandler(
            IDocumentRepository documentRepository)
        {
            this.documentRepository = documentRepository;
        }

        /// <summary>
        /// Handle the command.
        /// </summary>
        /// <param name="request"> The command.</param>
        /// <param name="cancellationToken"> The cancellation token.</param>
        /// <returns> The parsed document.</returns>
        public async Task<ErrorOr<(DappPDF, DocumentId)>> Handle(ParseDocumentCommand request, CancellationToken cancellationToken)
        {
            var doc = documentRepository.Get(request.DocumentId)!;

            var pdf = await DappPDF.Create(request.Data, doc.Name, doc.Url);
            return (pdf, doc.Id);
        }
    }
}
