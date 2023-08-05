using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API2.Controllers
{
    public class ErrorsController : ApiController
    {
        [Route("/error")]
        public IActionResult Error()
        {
            _ = HttpContext?.Features?.Get<IExceptionHandlerFeature>()?.Error;
            return Problem();
        }
    }
}
