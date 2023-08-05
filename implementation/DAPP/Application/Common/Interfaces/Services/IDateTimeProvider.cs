namespace Application.Common.Interfaces.Services
{
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Gets the current date and time in UTC.
        /// </summary>
        DateTime UtcNow { get; }
    }
}
