using Domain.Common.Models;

namespace Domain.PageAggregate.ValueObjects
{
    /// <summary>
    /// Value object for the page id.
    /// </summary>
    public class PageId : ValueObject
    {
        /// <summary>
        /// The value of the page id.
        /// </summary>
        public Guid Value { get; private set; }
        private PageId(Guid value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a unique page id.
        /// </summary>
        /// <returns></returns>
        public static PageId CreateUnique()
        {
            return new PageId(Guid.NewGuid());
        }

        /// <summary>
        /// Creates a page id from a guid.
        /// </summary>
        /// <param name="value"> The guid to create the page id from.</param>
        /// <returns> The page id.</returns>
        public static PageId Create(Guid value)
        {
            return new PageId(value);
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
