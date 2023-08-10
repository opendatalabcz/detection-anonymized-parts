using Domain.Common.Models;
using Domain.DocumentAggregate.ValueObjects;
using Domain.PageAggregate;

namespace Domain.DocumentAggregate
{
    public class Document : AggregateRoot<DocumentId>
    {
        public string Name { get; private set; } = null!;

        public int PageCount => Pages.Count;
        public string Url { get; private set; } = null!;

        public string Hash { get; private set; } = null!;
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

        protected Document() : base(DocumentId.CreateUnique()) // Required for EF
        {
        }

        public static Document Create(
            string name, string url, string hash, List<Page> pages)
        {
            return new(
                DocumentId.CreateUnique(),
                name, url, hash, pages);
        }
    }
}
