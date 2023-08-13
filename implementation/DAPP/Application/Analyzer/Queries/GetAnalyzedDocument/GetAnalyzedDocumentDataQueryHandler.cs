using Application.Common.Interfaces.Persistance;
using Application.Common.Interfaces.Services;
using Domain.Common.Entities.Analyzer;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Queries.GetAnalyzedDocument
{
    public class GetAnalyzedDocumentDataQueryHandler : IRequestHandler<GetAnalyzedDocumentDataQuery, ErrorOr<AnalyzedDocumentData>>
    {
        private readonly IFileHandleService fileHandleService;
        private readonly IDocumentRepository documentRepository;
        public GetAnalyzedDocumentDataQueryHandler(
            IFileHandleService fileHandleService,
            IDocumentRepository documentRepository)
        {
            this.fileHandleService = fileHandleService;
            this.documentRepository = documentRepository;
        }

        public async Task<ErrorOr<AnalyzedDocumentData>> Handle(GetAnalyzedDocumentDataQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var doc = documentRepository.Get(request.Id);
            if (doc is null)
            {
                return Domain.Common.Errors.Repository.EntityDoesNotExist;
            }

            if (doc.Pages is null || doc.PageCount == 0)
            {
                return Domain.Common.Errors.Analyzer.DocumentNotYetAnalyzed;
            }

            var data = new AnalyzedDocumentData(
                DocumentId: doc.Id,
                Url: doc.Url,
                ContainsAnonymizedData: doc.Pages.Any(p => p.AnonymizationResult > 0),
                AnonymizedPercentage: doc.Pages.Average(p => p.AnonymizationResult),
                PageCount: doc.PageCount,
                AnonymizedPercentagePerPage: doc.Pages.ToDictionary(p => p.PageNumber, p => p.AnonymizationResult),
                OriginalImages: doc.Pages.ToDictionary(p => p.PageNumber, p => fileHandleService.GetBytes(p.OriginalImageUrl).Result.Value),
                ResultImages: doc.Pages.ToDictionary(p => p.PageNumber, p => fileHandleService.GetBytes(p.ResultImageUrl).Result.Value)
            );

            return data;
        }
    }
}
