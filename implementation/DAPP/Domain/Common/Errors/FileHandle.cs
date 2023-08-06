using ErrorOr;

namespace Domain.Common.Errors
{

    public static partial class FileHandle
    {
        /// <summary>
        /// Representing an error when a pdf file cannot be loaded
        /// </summary>
        public static Error LoadingPdfError = Error.Failure
            (
                code: "901",
                description: "Failed to load a pdf."
            );
    }
}
