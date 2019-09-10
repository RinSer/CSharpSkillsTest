using System;
using TestSolution.Data;

namespace TestSolution.Repositories
{
    public class DocumentRepository : RepositoryBase<Document>
    {
        public override Document Get(long documentId)
        {
            var document = base.Get(documentId);

            if (document.FirstReadTime == null)
            {
                document.FirstReadTime = DateTime.Now;
                Update(document, false);
            }

            return document;
        }

        public override Document Archive(long documentId)
        {
            var document = base.Get(documentId);

            if (document.FirstReadTime == null)
            {
                document.FirstReadTime = DateTime.Now;
                document.ArchiveTime = document.FirstReadTime;
                Update(document, false);
            }

            return base.Archive(documentId);
        }

        protected override void UpdateMutableFields(Document oldDocument, Document newDocument)
        {
            oldDocument.Title = newDocument.Title;
            oldDocument.Author = newDocument.Author;
        }
    }
}
