using Application.Analyzer.Commands.AnalyzeDocument;
using Application.Analyzer.Commands.ParseDocument;
using Application.Analyzer.Commands.RegisterDocument;
using Application.Analyzer.Queries.GetAnalyzedDocument;
using Contracts.Analyzer;
using Contracts.Results;
using Domain.DocumentAggregate.ValueObjects;
using Domain.PageAggregate.ValueObjects;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API2.Controllers
{
    /// <summary>
    /// Analyzer controller.
    /// </summary>
    public class AnalyzerController : ApiController
    {
        private readonly IMapper mapper;
        private readonly ISender mediator;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AnalyzerController(IMapper mapper, ISender mediator)
        {
            this.mapper = mapper;
            this.mediator = mediator;
        }

        /// <summary>
        /// Analyze a document.
        /// </summary>
        /// <param name="request"> The request.</param>
        /// <returns> The response.</returns>
        [HttpPost]
        [Route("/analyze")]
        public async Task<IActionResult> Analyze([FromBody] AnalyzeDocumentRequest request)
        {

            var registerCmd = mapper.Map<RegisterDocumentCommand>(request);

            if (registerCmd is null)
            {
                return BadRequest();
            }

            var registerResponse = await mediator.Send(registerCmd);
            if (registerResponse.IsError)
            {
                return Problem(registerResponse.Errors);
            }

            var parseCmd = new ParseDocumentCommand(registerResponse.Value.Item1, registerResponse.Value.Item2);
            var parseResponse = await mediator.Send(parseCmd);

            if (parseResponse.IsError)
            {
                return Problem(parseResponse.Errors);
            }

            var cmd = new AnalyzeDocumentCommand(parseResponse.Value.Item1, parseResponse.Value.Item2, request.ReturnImages);
            var analyzeResponse = await mediator.Send(cmd);

            if (analyzeResponse.IsError)
            {
                return Problem(analyzeResponse.Errors);
            }

            var q = new GetAnalyzedDocumentDataQuery(analyzeResponse.Value.Id);
            var response = await mediator.Send(q);
            if (response.IsError)
            {
                return Problem(response.Errors);
            }
            var res = new AnalyzeDocumentResponse
                (
                   DocumentId: response.Value.DocumentId.Value,
                   Url: response.Value.Url,
                   ContainsAnonymizedData: response.Value.ContainsAnonymizedData,
                   AnonymizedPercentage: response.Value.AnonymizedPercentage,
                   PageCount: response.Value.PageCount,
                   AnonymizedPercentagePerPage: response.Value.AnonymizedPercentagePerPage,
                   OriginalImages: response.Value.OriginalImages,
                   ResultImages: response.Value.ResultImages
                );
            if (!request.ReturnImages)
            {
                res = res with { OriginalImages = null };
                res = res with { ResultImages = null };
            }
            return Ok(res);
        }

        /// <summary>
        /// Gets the results of a document.
        /// </summary>
        /// <param name="request"> The request.</param>
        /// <returns> The response.</returns>
        [HttpGet]
        [Route("/results")]
        public async Task<IActionResult> GetResults([FromBody] GetDocumentPagesRequest request)
        {
            var q = new GetAnalyzedDocumentDataQuery(DocumentId.Create(new Guid(request.DocumentId)));
            var response = await mediator.Send(q);
            if (response.IsError)
            {
                return Problem(response.Errors);
            }

            Dictionary<int, Dictionary<string, byte[]>> pages = new();

            foreach (var kvp in response.Value.OriginalImages)
            {
                int pageIndex = kvp.Key;
                Dictionary<string, byte[]> images = new()
        {
            { "Original", kvp.Value },
            { "Result", response.Value.ResultImages[pageIndex] }
        };
                pages.Add(pageIndex, images);
            }

            var res = new GetDocumentPagesResponse(
                DocumentId: response.Value.DocumentId.Value,
                Url: response.Value.Url,
                Pages: pages);

            return Ok(res);
        }
    }
}
