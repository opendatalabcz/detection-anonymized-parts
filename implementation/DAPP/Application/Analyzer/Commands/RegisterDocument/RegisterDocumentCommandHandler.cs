using Application.Common.Interfaces.Persistance;
using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.RegisterDocument
{
    public class RegisterDocumentCommandHandler : IRequestHandler<RegisterDocumentCommand, ErrorOr<DocumentId>>
    {

        private readonly IDocumentRepository documentRepository;

        public RegisterDocumentCommandHandler(
            IDocumentRepository documentRepository)
        {
            this.documentRepository = documentRepository;
        }

        public async Task<ErrorOr<DocumentId>> Handle(RegisterDocumentCommand request, CancellationToken cancellationToken)
        {

            // The contract name is the last part of the path (e.g. "*\contract1.pdf")
            var contractName = request.FileLocation.Split("/").Last().Split(".").First();

            var document = documentRepository.Get(contractName);
            if (document is null)
            {
                document = Document.Create(contractName, request.FileLocation, new());

                return documentRepository.Add(document);
            }

            else
            {
                return document.Id;
            }
        }
    }
}
