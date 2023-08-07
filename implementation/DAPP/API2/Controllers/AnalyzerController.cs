using Application.Analyzer.Commands.AnalyzeDocument;
using Application.Analyzer.Commands.ParseDocument;
using Application.Analyzer.Commands.RegisterDocument;
using Contracts.Analyzer;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API2.Controllers
{
    public class AnalyzerController : ApiController
    {
        private readonly IMapper mapper;
        private readonly ISender mediator;

        public AnalyzerController(IMapper mapper, ISender mediator)
        {
            this.mapper = mapper;
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("/analyze")]
        public async Task<IActionResult> Analyze([FromBody] AnalyzeDocumentRequest request)
        {

            var registerCmd = mapper.Map<RegisterDocumentCommand>(request);
            var registerResponse = await mediator.Send(registerCmd);
            if (registerResponse.IsError)
            {
                return Problem(registerResponse.Errors);
            }

            var parseCmd = new ParseDocumentCommand(registerResponse.Value);
            var parseResponse = await mediator.Send(parseCmd);

            if (parseResponse.IsError)
            {
                return Problem(parseResponse.Errors);
            }

            var cmd = new AnalyzeDocumentCommand(parseResponse.Value.Item1, parseResponse.Value.Item2, request.ReturnImages);
            var response = await mediator.Send(cmd);

            return response.Match(
                document => Ok(mapper.Map<AnalyzeDocumentResponse>(document)),
                Problem);
        }
    }
}
