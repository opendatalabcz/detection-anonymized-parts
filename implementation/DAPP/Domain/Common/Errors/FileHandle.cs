using ErrorOr;

namespace Domain.Common.Errors
{

    /// <summary>
    /// Representing errors that can occur when handling files
    /// </summary>
    public static partial class FileHandle
    {
        /// <summary>
        /// Representing an error when a pdf file cannot be loaded
        /// </summary>
        public static readonly Error LoadingPdfError = Error.Validation
            (
                code: "901",
                description: "Failed to load a pdf."
            );
    }
}
