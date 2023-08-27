using Domain.Common.Models;
using Domain.DocumentAggregate.ValueObjects;
using Domain.PageAggregate;

namespace Domain.DocumentAggregate
{
    /// <summary>
    /// The document aggregate root.
    /// </summary>
    public class Document : AggregateRoot<DocumentId>
    {
        /// <summary>
        /// The name of the document.
        /// </summary>
        public string Name { get; private set; } = null!;

        /// <summary>
        /// The number of pages in the document.
        /// </summary>
        public int PageCount => Pages.Count;
        /// <summary>
        /// The url of the document.
        /// </summary>
        public string Url { get; private set; } = null!;
        /// <summary>
        /// The hash of the document.
        /// </summary>
        public string Hash { get; private set; } = null!;
        /// <summary>
        /// The pages of the document.
        /// </summary>
        public virtual List<Page> Pages { get; private set; } = null!;

        private Document(DocumentId id,
                       string name,
                       string url,
                       string hash,
                       List<Page> pages)
            : base(id)
        {
            Name = name;
            Pages = pages;
            Hash = hash;
            Url = url;
        }

        /// <summary>
        /// Required for EF
        /// </summary>
        protected Document() : base(DocumentId.CreateUnique()) // Required for EF
        {
        }

        /// <summary>
        /// Creates a new document.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="hash"></param>
        /// <param name="pages"></param>
        /// <returns></returns>
        public static Document Create(
            string name, string url, string hash, List<Page> pages)
        {
            return new(
                DocumentId.CreateUnique(),
                name, url, hash, pages);
        }
    }
}
