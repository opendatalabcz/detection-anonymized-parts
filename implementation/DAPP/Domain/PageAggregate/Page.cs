using Domain.Common.Models;
using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;
using Domain.PageAggregate.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Domain.PageAggregate
{
    public class Page : AggregateRoot<PageId>
    {
        public string OriginalImageUrl { get; private set; } = null!;
        public string ResultImageUrl { get; private set; } = null!;
        public int PageNumber { get; private set; }
        public virtual Document Document { get; private set; } = null!;

        [ExcludeFromCodeCoverage]
        public DocumentId DocumentId { get; private set; } = null!;
        public float AnonymizationResult { get; private set; }
        public Page(PageId id,
            Document document,
            int pageNumber,
            string originalImageUrl,
            string resultImageUrl,
            float anonymizationResult) : base(id)
        {
            Document = document;
            PageNumber = pageNumber;
            OriginalImageUrl = originalImageUrl;
            ResultImageUrl = resultImageUrl;
            AnonymizationResult = anonymizationResult;
        }


        protected Page() : base(PageId.CreateUnique()) // Required for EF
        {
        }
        public static Page Create(
            Document document,
            int pageNumber,
            string originalImageUrl,
            string resultImageUrl,
            float anonymizationResult)
        {
            return new(
                PageId.CreateUnique(),
                document,
                pageNumber,
                originalImageUrl,
                resultImageUrl,
                anonymizationResult);
        }
    }
}
