using Application.Analyzer.Commands.AnalyzeDocument;
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
        public async Task<IActionResult> Analyze(AnalyzeDocumentRequest request)
        {
            var cmd = mapper.Map<AnalyzeDocumentCommand>(request);
            var response = await mediator.Send(cmd);

            return response.Match(
                document => Ok(mapper.Map<AnalyzeDocumentResponse>(document)),
                Problem);
        }
    }
}
