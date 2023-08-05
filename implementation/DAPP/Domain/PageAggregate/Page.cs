using Domain.Common.Models;
using Domain.DocumentAggregate;
using Domain.PageAggregate.ValueObjects;

namespace Domain.PageAggregate
{
    public class Page : AggregateRoot<PageId>
    {
        public string OriginalImageUrl { get; private set; }
        public string ResultImageUrl { get; private set; }
        public Document Document { get; private set; }
        public float AnonymizationResult { get; private set; }
        public Page(PageId id,
            Document document,
            string originalImageUrl,
            string resultImageUrl,
            float anonymizationResult) : base(id)
        {
            Document = document;
            OriginalImageUrl = originalImageUrl;
            ResultImageUrl = resultImageUrl;
            AnonymizationResult = anonymizationResult;
        }

        public static Page Create(
            Document document,
            string originalImageUrl,
            string resultImageUrl,
            float anonymizationResult)
        {
            return new(
                PageId.CreateUnique(),
                document,
                originalImageUrl,
                resultImageUrl,
                anonymizationResult);
        }
    }
}
