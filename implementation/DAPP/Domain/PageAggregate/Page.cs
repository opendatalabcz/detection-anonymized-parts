using Domain.Common.Models;
using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;
using Domain.PageAggregate.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Domain.PageAggregate
{
    /// <summary>
    /// The page aggregate root.
    /// </summary>
    public class Page : AggregateRoot<PageId>
    {
        /// <summary>
        /// The original image url.
        /// </summary>
        public string OriginalImageUrl { get; private set; } = null!;
        /// <summary>
        /// The result image url.
        /// </summary>
        public string ResultImageUrl { get; private set; } = null!;
        /// <summary>
        /// The page number.
        /// </summary>
        public int PageNumber { get; private set; }
        /// <summary>
        /// The document.
        /// </summary>
        public virtual Document Document { get; private set; } = null!;

        /// <summary>
        /// The document id.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public DocumentId DocumentId { get; private set; } = null!;
        /// <summary>
        /// The anonymization result.
        /// </summary>
        public float AnonymizationResult { get; private set; }
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="id"> The id.</param>
        /// <param name="document"> The document.</param>
        /// <param name="pageNumber"> The page number.</param>
        /// <param name="originalImageUrl"> The original image url.</param>
        /// <param name="resultImageUrl"> The result image url.</param>
        /// <param name="anonymizationResult"> The anonymization result.</param>
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


        /// <summary>
        /// Required for EF
        /// </summary>
        protected Page() : base(PageId.CreateUnique()) // Required for EF
        {
        }

        /// <summary>
        /// Creates a new page.
        /// </summary>
        /// <param name="document"> The document.</param>
        /// <param name="pageNumber"> The page number.</param>
        /// <param name="originalImageUrl"> The original image url.</param>
        /// <param name="resultImageUrl"> The result image url.</param>
        /// <param name="anonymizationResult"> The anonymization result.</param>
        /// <returns></returns>
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
