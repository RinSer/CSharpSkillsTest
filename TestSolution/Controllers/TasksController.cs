using Microsoft.AspNetCore.Mvc;
using TestSolution.Data;
using TestSolution.Interfaces;

namespace TestSolution.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : BaseController<Task>
    {
        public TasksController(IRepository<Task> repository) : base(repository) { }
    }
}