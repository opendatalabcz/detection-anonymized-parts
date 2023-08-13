using Application.Analyzer.Commands.AnalyzeDocument;
using Application.Analyzer.Queries.GetAnalyzedDocument;
using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Unit.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public async Task AnalyzeDocumentHandler_ReturnError_EntityDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DappDbContext>()
                .Options;
            var context = new DappDbContext(options);
            var DocumentRepository = new DocumentRepository(context);
            var PageRepository = new PageRepository(context);
            var handler = new AnalyzeDocumentCommandHandler(PageRepository, DocumentRepository);
            // Act
            var r = await handler.Handle(new AnalyzeDocumentCommand(null, DocumentId.CreateUnique(), false), CancellationToken.None);
            // Assert
            Assert.True(r.IsError);
            Assert.Equal(Domain.Common.Errors.Repository.EntityDoesNotExist, r.FirstError);
        }

        [Fact]
        public async Task GetAnalyzedDocumentHandler_ReturnError_DocumentNotYetAnalyzed()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DappDbContext>()
                .Options;
            var context = new DappDbContext(options);
            var DocumentRepository = new DocumentRepository(context);
            var handler = new GetAnalyzedDocumentDataQueryHandler(new FileHandleService(), DocumentRepository);
            var d = Document.Create("Test", "Test", "Test", null);
            var id = DocumentRepository.Add(d);
            // Act
            var r = await handler.Handle(new GetAnalyzedDocumentDataQuery(id), CancellationToken.None);
            // Assert
            Assert.True(r.IsError);
            Assert.Equal(Domain.Common.Errors.Analyzer.DocumentNotYetAnalyzed, r.FirstError);
        }
    }
}
