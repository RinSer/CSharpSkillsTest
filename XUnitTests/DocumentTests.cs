using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using TestSolution.Controllers;
using TestSolution.Data;
using TestSolution.Repositories;
using Xunit;

namespace XUnitTests
{
    /// <summary>
    /// Тесты для документов
    /// </summary>
    public class DocumentTests
    {
        private readonly DocumentsController _docsController;

        public DocumentTests()
        {
            _docsController = new DocumentsController(new DocumentRepository());
        }

        /// <summary>
        /// Документ должен создаваться
        /// </summary>
        [Fact]
        public void ShouldCreateNewDocument()
        {
            var testDocument = MakeNew(0);
            var response = _docsController.Create(testDocument);
            var result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var newDocument = Assert.IsType<Document>(result.Value);
            Assert.True(newDocument.Id > 0);
            Assert.Null(newDocument.FirstReadTime);
            Assert.Null(newDocument.UpdateTime);
            Assert.Null(newDocument.ArchiveTime);
            Assert.Equal(testDocument.Title, newDocument.Title);
            Assert.Equal(testDocument.Author, newDocument.Author);
        }

        /// <summary>
        /// Уже созданный документ должен возвращаться по его идентификатору,
        /// поле "время первого прочтения" должно заполняться при первом получении документа
        /// </summary>
        [Fact]
        public void ShouldGetDocumentById()
        {
            var testDocument = MakeNew(1);
            var response = _docsController.Create(testDocument);
            var result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var newDocument = Assert.IsType<Document>(result.Value);
            Assert.Null(newDocument.FirstReadTime);

            response = _docsController.Get(newDocument.Id);
            result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var document = Assert.IsType<Document>(result.Value);
            Assert.Equal(newDocument.Id, document.Id);
            Assert.Equal(testDocument.Title, document.Title);
            Assert.Equal(testDocument.Author, document.Author);
            Assert.NotNull(document.FirstReadTime);

            var testFirstTime = document.FirstReadTime;
            response = _docsController.Get(newDocument.Id);
            result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            document = Assert.IsType<Document>(result.Value);
            Assert.Equal(testFirstTime, document.FirstReadTime);
        }

        /// <summary>
        /// Существующий документ должен обновляться
        /// </summary>
        [Fact]
        public void ShouldUpdateDocument()
        {
            var testDocument = MakeNew(2);
            var response = _docsController.Create(testDocument);
            var result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var newDocument = Assert.IsType<Document>(result.Value);
            Assert.Null(newDocument.UpdateTime);

            newDocument.Title = "Updated document title";
            newDocument.Author = "Updated author";
            response = _docsController.Update(newDocument);
            result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var updatedDocument = Assert.IsType<Document>(result.Value);
            Assert.NotNull(updatedDocument.UpdateTime);
            Assert.Equal(newDocument.Id, updatedDocument.Id);
            Assert.Equal(newDocument.Title, updatedDocument.Title);
            Assert.Equal(newDocument.Author, updatedDocument.Author);
        }

        /// <summary>
        /// Существующий документ должен отправляться в архив, 
        /// если он был хоть раз просмотрен, то значения
        /// полей "время первого получения" и "время отправки в архив"
        /// не должны совпадать
        /// </summary>
        [Fact]
        public void ShouldArchiveDocument()
        {
            var testDocument = MakeNew(3);
            var response = _docsController.Create(testDocument);
            var result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var newDocument = Assert.IsType<Document>(result.Value);
            Assert.Null(newDocument.ArchiveTime);

            _docsController.Get(newDocument.Id);

            response = _docsController.Archive(newDocument.Id);
            result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var archivedDocument = Assert.IsType<Document>(result.Value);
            Assert.NotNull(archivedDocument.ArchiveTime);
            Assert.Equal(newDocument.Id, archivedDocument.Id);
            Assert.NotEqual(archivedDocument.ArchiveTime, archivedDocument.FirstReadTime);
        }

        /// <summary>
        /// Существующий документ должен отправляться в архив, 
        /// если он не был ни разу просмотрен, то значения
        /// полей "время первого получения" и "время отправки в архив"
        /// должны совпадать
        /// </summary>
        [Fact]
        public void ShouldArchiveDocumentAndSetFirstReadTime()
        {
            var testDocument = MakeNew(3);
            var response = _docsController.Create(testDocument);
            var result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var newDocument = Assert.IsType<Document>(result.Value);
            Assert.Null(newDocument.ArchiveTime);

            response = _docsController.Archive(newDocument.Id);
            result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var archivedDocument = Assert.IsType<Document>(result.Value);
            Assert.NotNull(archivedDocument.ArchiveTime);
            Assert.Equal(newDocument.Id, archivedDocument.Id);
            Assert.Equal(archivedDocument.ArchiveTime, archivedDocument.FirstReadTime);
        }

        private Document MakeNew(int docNumber)
        {
            return new Document
            {
                Title = $"TEST_DOC_{docNumber}",
                Author = $"Test Author #{docNumber}"
            };
        }
    }
}
