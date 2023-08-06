using Application.Common.Interfaces.Persistance;
using Application.Common.Interfaces.Services;
using DAPPAnalyzer.Models;
using Domain.DocumentAggregate.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.ParseDocument
{
    public class ParseDocumentCommandHandler : IRequestHandler<ParseDocumentCommand, ErrorOr<(DappPDF, DocumentId)>>
    {
        private readonly IDocumentRepository documentRepository;

        private readonly IFileHandleService fileHandleService;

        public ParseDocumentCommandHandler(
            IDocumentRepository documentRepository,
            IFileHandleService fileHandleService)
        {
            this.documentRepository = documentRepository;
            this.fileHandleService = fileHandleService;
        }

        public async Task<ErrorOr<(DappPDF, DocumentId)>> Handle(ParseDocumentCommand request, CancellationToken cancellationToken)
        {
            var doc = documentRepository.Get(request.DocumentId);

            var data = await fileHandleService.GetBytes(doc.Url);

            if (data.IsError)
            {
                return data.Errors;
            }

            var pdf = await DappPDF.Create(data.Value, doc.Name, doc.Url);
            return (pdf, doc.Id);
        }
    }
}
