using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API2.Controllers
{
    /// <summary>
    /// Errors controller.
    /// </summary>
    public class ErrorsController : ApiController
    {
        /// <summary>
        /// Error.
        /// </summary>
        /// <returns> The response.</returns>
        [Route("/error")]
        public IActionResult Error()
        {
            _ = HttpContext?.Features?.Get<IExceptionHandlerFeature>()?.Error;
            return Problem();
        }
    }
}
