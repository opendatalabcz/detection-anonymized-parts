using Application.Analyzer.Commands.AnalyzeDocument;
using Contracts.Analyzer;
using Domain.DocumentAggregate;
using Mapster;

namespace API2.Common.Mappings
{
    public class AnalyzerMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AnalyzeDocumentRequest, AnalyzeDocumentCommand>()
                .Map(dest => dest.FileLocation, src => src.FileLocation)
                .Map(dest => dest.ReturnImages, src => src.ReturnImages);

            config.NewConfig<Document, AnalyzeDocumentResponse>();
        }

    }
}
