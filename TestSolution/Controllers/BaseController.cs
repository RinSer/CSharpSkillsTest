using System;
using Microsoft.AspNetCore.Mvc;
using TestSolution.Data;
using TestSolution.Interfaces;

namespace TestSolution.Controllers
{
    [ApiController]
    public abstract class BaseController<TEntity> : ControllerBase where TEntity : SystemBase
    {
        private readonly IRepository<TEntity> _repository;

        protected BaseController(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public ActionResult<TEntity> Get(long id)
        {
            return Execute<long>(id, _repository.Get);
        }

        [HttpPost]
        public ActionResult<TEntity> Create([FromBody] TEntity entity)
        {
            return Execute<TEntity>(entity, _repository.Create);
        }

        [HttpPut]
        public ActionResult<TEntity> Update([FromBody] TEntity entity)
        {
            return Execute<TEntity>(entity, _repository.Update);
        }

        [HttpDelete("{id}")]
        public ActionResult<TEntity> Archive(long id)
        {
            return Execute<long>(id, _repository.Archive);
        }

        private ActionResult<TEntity> Execute<T>(T argument, Func<T, TEntity> method)
        {
            try
            {
                var result = method(argument);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
