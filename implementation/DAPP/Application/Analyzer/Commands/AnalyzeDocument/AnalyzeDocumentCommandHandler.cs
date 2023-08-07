using Application.Common.Interfaces.Persistance;
using Domain.DocumentAggregate;
using Domain.PageAggregate;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.AnalyzeDocument
{
    public class AnalyzeDocumentCommandHandler : IRequestHandler<AnalyzeDocumentCommand, ErrorOr<Document>>
    {
        private readonly IMediator mediator;
        private readonly IPageRepository pageRepository;
        private readonly IDocumentRepository documentRepository;
        public AnalyzeDocumentCommandHandler(
            IMediator mediator,
            IPageRepository pageRepository,
            IDocumentRepository documentRepository)
        {
            this.mediator = mediator;
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
                    result.AnonymizedPercentagePerPage[item.Key]);

                pageRepository.Add(p);
            }

            return documentRepository.Get(doc.Id)!;
        }
    }
}
