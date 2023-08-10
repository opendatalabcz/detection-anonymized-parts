using Application.Common.Interfaces.Persistance;
using DAPPAnalyzer.Models;
using Domain.DocumentAggregate.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.ParseDocument
{
    public class ParseDocumentCommandHandler : IRequestHandler<ParseDocumentCommand, ErrorOr<(DappPDF, DocumentId)>>
    {
        private readonly IDocumentRepository documentRepository;


        public ParseDocumentCommandHandler(
            IDocumentRepository documentRepository)
        {
            this.documentRepository = documentRepository;
        }

        public async Task<ErrorOr<(DappPDF, DocumentId)>> Handle(ParseDocumentCommand request, CancellationToken cancellationToken)
        {
            var doc = documentRepository.Get(request.DocumentId)!;

            var pdf = await DappPDF.Create(request.Data, doc.Name, doc.Url);
            return (pdf, doc.Id);
        }
    }
}
