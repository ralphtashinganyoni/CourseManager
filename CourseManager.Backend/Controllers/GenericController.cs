using CourseManager.Application.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T> : ControllerBase where T : class
    {
        private readonly IBaseRepository<T> _repository;

        public GenericController(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<T>> GetAll()
        {
            var items = _repository.Find();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<T> GetById([FromRoute] Guid id)
        {
            var item = _repository.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        [Authorize]
        public ActionResult<T> Create([FromBody] T item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdItem = _repository.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = GetKeyId(createdItem) }, createdItem);
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<T> Update([FromRoute] Guid id, [FromBody] T item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedItem = _repository.Update(id, item);
                return Ok(updatedItem);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete([FromRoute] Guid id)
        {
            _repository.Delete(id);
            return Ok();
        }

        private Guid GetKeyId(T item)
        {
            var keyProperty = item.GetType().GetProperty("Id");
            if (keyProperty != null)
            {
                return (Guid)keyProperty.GetValue(item);
            }
            throw new System.Exception("Key property 'Id' not found on type " + typeof(T).Name);
        }
    }
}
