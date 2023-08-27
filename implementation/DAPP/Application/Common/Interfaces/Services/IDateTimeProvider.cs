namespace Application.Common.Interfaces.Services
{
    /// <summary>
    /// Interface for the date time provider.
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Gets the current date and time in UTC.
        /// </summary>
        DateTime UtcNow { get; }
    }
}
