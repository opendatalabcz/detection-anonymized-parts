using ErrorOr;
namespace API.Errors
{
    public static partial class ApiErrors
    {
        public static Error LoadingPdfError = Error.Failure
            (
                code: "901",
                description: "Failed to load pdf"
            );
    }
}
