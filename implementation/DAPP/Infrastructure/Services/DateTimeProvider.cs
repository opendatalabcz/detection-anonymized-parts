using Application.Common.Interfaces.Services;

namespace Infrastructure.Services
{
    /// <summary>
    /// Implementation of the date time provider.
    /// </summary>
    public class DateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// Gets the current date and time in UTC.
        /// </summary>
        public DateTime UtcNow => DateTime.Now;
    }
}
