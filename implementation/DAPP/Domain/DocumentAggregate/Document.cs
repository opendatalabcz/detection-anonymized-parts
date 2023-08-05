using Domain.Common.Models;
using Domain.DocumentAggregate.ValueObjects;
using Domain.PageAggregate;

namespace Domain.DocumentAggregate
{
    public class Document : AggregateRoot<DocumentId>
    {
        public string Name { get; private set; }

        public int PageCount => Pages.Count;
        public string Url { get; private set; }

        public List<Page> Pages { get; private set; }

        private Document(DocumentId id,
                       string name,
                       string url,
                       List<Page> pages)
            : base(id)
        {
            Name = name;
            Pages = pages;
            Url = url;
        }

        public static Document Create(
            string name, string url, List<Page> pages)
        {
            return new(
                DocumentId.CreateUnique(),
                name, url, pages);
        }
    }
}
