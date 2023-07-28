using API.IServices;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;
        public TodoController(ITodoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Todo>> Get()
        {
            return await _service.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<Todo> Detail(Guid id)
        {
            return await _service.GetById(id);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Todo todo)
        {
            await _service.Add(todo);
            return Ok(todo);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Todo todo)
        {
            await _service.Update(todo);
            return Ok(todo);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
