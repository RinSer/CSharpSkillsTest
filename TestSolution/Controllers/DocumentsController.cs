
using Microsoft.AspNetCore.Mvc;
using TestSolution.Data;
using TestSolution.Interfaces;

namespace TestSolution.Controllers
{
    [Route("api/[controller]")]
    public class DocumentsController : BaseController<Document>
    {
        public DocumentsController(IRepository<Document> repository) : base(repository) { }
    }
}