using Domain.Common.Models;

namespace Domain.DocumentAggregate.ValueObjects
{
    public sealed class DocumentId : ValueObject
    {
        public Guid Value { get; private set; }
        private DocumentId(Guid value)
        {
            Value = value;
        }

        public static DocumentId CreateUnique()
        {
            return new DocumentId(Guid.NewGuid());
        }

        public static DocumentId Create(Guid value)
        {
            return new DocumentId(value);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
