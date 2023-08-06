using Domain.Common.Models;
using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;
using Domain.PageAggregate.ValueObjects;

namespace Domain.PageAggregate
{
    public class Page : AggregateRoot<PageId>
    {
        public string OriginalImageUrl { get; private set; }
        public string ResultImageUrl { get; private set; }
        public int PageNumber { get; private set; }
        public Document Document { get; private set; }

        public DocumentId DocumentId { get; private set; }
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


        private Page() : base(PageId.CreateUnique()) // Required for EF
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
