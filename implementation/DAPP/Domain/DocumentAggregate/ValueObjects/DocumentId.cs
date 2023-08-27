using Domain.Common.Models;

namespace Domain.DocumentAggregate.ValueObjects
{
    /// <summary>
    /// Value object for the document id.
    /// </summary>
    public sealed class DocumentId : ValueObject
    {
        /// <summary>
        /// The value of the document id.
        /// </summary>
        public Guid Value { get; private set; }
        private DocumentId(Guid value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a unique document id.
        /// </summary>
        /// <returns> The document id.</returns>
        public static DocumentId CreateUnique()
        {
            return new DocumentId(Guid.NewGuid());
        }

        /// <summary>
        /// Creates a document id from a guid.
        /// </summary>
        /// <param name="value"> The guid.</param>
        /// <returns> The document id.</returns>
        public static DocumentId Create(Guid value)
        {
            return new DocumentId(value);
        }

        /// <summary>
        /// Gets the equality components.
        /// </summary>
        /// <returns> Yields the equality components.</returns>
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
