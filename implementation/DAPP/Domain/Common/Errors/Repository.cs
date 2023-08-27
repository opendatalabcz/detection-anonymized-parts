using ErrorOr;

namespace Domain.Common.Errors
{
    /// <summary>
    /// Class containing all errors that can occur in the repository.
    /// </summary>
    public static partial class Repository
    {
        /// <summary>
        /// Representing an error when a pdf file cannot be loaded
        /// </summary>
        public static readonly Error EntityDoesNotExist = Error.Failure
            (
                code: "801",
                description: "Entity with given Id is not in the database."
            );
    }
}
