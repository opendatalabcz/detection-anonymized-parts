﻿using Application.Common.Interfaces.Persistance;
using Application.Common.Interfaces.Services;
using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;
using ErrorOr;
using MediatR;
using System.Security.Cryptography;

namespace Application.Analyzer.Commands.RegisterDocument
{
    /// <summary>
    /// Command Handler to register a document.
    /// </summary>
    public class RegisterDocumentCommandHandler : IRequestHandler<RegisterDocumentCommand, ErrorOr<(DocumentId, byte[])>>
    {

        private readonly IDocumentRepository documentRepository;
        private readonly IFileHandleService fileHandleService;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="documentRepository"> The document repository.</param>
        /// <param name="fileHandleService"> The file handle service.</param>
        public RegisterDocumentCommandHandler(
            IDocumentRepository documentRepository, IFileHandleService fileHandleService)
        {
            this.documentRepository = documentRepository;
            this.fileHandleService = fileHandleService;
        }

        /// <summary>
        /// Handle the command.
        /// </summary>
        /// <param name="request"> The command.</param>
        /// <param name="cancellationToken"> The cancellation token.</param>
        /// <returns> The document id and the document data.</returns>
        public async Task<ErrorOr<(DocumentId, byte[])>> Handle(RegisterDocumentCommand request, CancellationToken cancellationToken)
        {

            // The contract name is the last part of the path (e.g. "*\contract1.pdf")
            var contractName = request.FileLocation.Split("/").Last().Split(".").First();
            var document = await fileHandleService.GetBytes(request.FileLocation);

            if (document.IsError)
            {
                return document.Errors;
            }

            // calculate hash of the document
            var hash = SHA256.HashData(document.Value);
            var hashString = Convert.ToBase64String(hash);

            var entity = documentRepository.GetByHash(hashString);

            if (entity is null)
            {
                entity = Document.Create(contractName, request.FileLocation, hashString, new());

                documentRepository.Add(entity);
                return (entity.Id, document.Value);
            }

            else
            {
                return (entity.Id, document.Value);
            }
        }
    }
}
