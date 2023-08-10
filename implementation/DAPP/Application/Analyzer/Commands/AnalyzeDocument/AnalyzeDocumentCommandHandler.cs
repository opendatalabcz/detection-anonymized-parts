using Application.Common.Interfaces.Persistance;
using Domain.DocumentAggregate;
using Domain.PageAggregate;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.AnalyzeDocument
{
    public class AnalyzeDocumentCommandHandler : IRequestHandler<AnalyzeDocumentCommand, ErrorOr<Document>>
    {
        private readonly IPageRepository pageRepository;
        private readonly IDocumentRepository documentRepository;
        public AnalyzeDocumentCommandHandler(
            IPageRepository pageRepository,
            IDocumentRepository documentRepository)
        {
            this.pageRepository = pageRepository;
            this.documentRepository = documentRepository;
        }

        public async Task<ErrorOr<Document>> Handle(AnalyzeDocumentCommand request, CancellationToken cancellationToken)
        {
            var doc = documentRepository.Get(request.DocumentId);
            if (doc is null)
            {
                return Domain.Common.Errors.Repository.EntityDoesNotExist;
            }
            if (doc.Pages is not null && doc.PageCount > 0)
            {
                return doc;
            }

            var pdf = request.DappPdf;

            var result = await DAPPAnalyzer.Services.PDFAnalyzer.AnalyzeAsync(pdf);


            foreach (var item in result.OriginalImages)
            {
                var originalImageUrl = pageRepository.SaveImage(item.Value);
                var resultImageUrl = pageRepository.SaveImage(result.ResultImages[item.Key]);

                var p = Page.Create(
                    doc,
                    item.Key,
                    originalImageUrl,
                    resultImageUrl,
                    result.AnonymizedPercentagePerPage[item.Key - 1]);

                pageRepository.Add(p);
            }

            return documentRepository.Get(doc.Id)!;
        }
    }
}
