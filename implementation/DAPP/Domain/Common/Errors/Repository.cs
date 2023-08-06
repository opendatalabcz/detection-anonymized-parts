using ErrorOr;

namespace Domain.Common.Errors
{
    public static partial class Repository
    {
        /// <summary>
        /// Representing an error when a pdf file cannot be loaded
        /// </summary>
        public static Error EntityDoesNotExist = Error.Failure
            (
                code: "801",
                description: "Entity with given Id is not in the database."
            );
    }
}
