using ErrorOr;

namespace Domain.Common.Errors
{
    public static partial class Analyzer
    {
        /// <summary>
        /// Representing an error when a pdf file cannot be loaded
        /// </summary>
        public static Error DocumentNotYetAnalyzed = Error.NotFound
            (
                code: "701",
                description: "The document has not been analyzed yet."
            );
    }
}