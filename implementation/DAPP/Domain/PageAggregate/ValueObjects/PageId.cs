using Domain.Common.Models;

namespace Domain.PageAggregate.ValueObjects
{
    public class PageId : ValueObject
    {
        public Guid Value { get; private set; }
        private PageId(Guid value)
        {
            Value = value;
        }

        public static PageId CreateUnique()
        {
            return new PageId(Guid.NewGuid());
        }

        public static PageId Create(Guid value)
        {
            return new PageId(value);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
