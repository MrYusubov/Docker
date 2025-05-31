using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestingTask.Models;
using TestingTask.Services;

namespace TestingTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private readonly IHouseService _service;

        public HouseController(IHouseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var houses = await _service.GetAllAsync();
            return Ok(houses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var house = await _service.GetByIdAsync(id);
            if (house == null)
                return NotFound();
            return Ok(house);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] House house)
        {
            var created = await _service.AddAsync(house);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] House house)
        {
            if (id != house.Id)
                return BadRequest();

            var updated = await _service.UpdateAsync(house);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
