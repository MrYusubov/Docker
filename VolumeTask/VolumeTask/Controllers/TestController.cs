using Microsoft.AspNetCore.Mvc;
using VolumeTask.Services;

namespace VolumeTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly FileLogger _logger;

        public TestController(FileLogger logger)
        {
            _logger = logger;
        }

        [HttpGet("success")]
        public async Task<IActionResult> Success()
        {
            await _logger.LogInfoAsync("Success endpoint called.");
            return Ok("Success");
        }

        [HttpGet("fail")]
        public async Task<IActionResult> Fail()
        {
            try
            {
                throw new Exception("Some unexpected error");
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Exception: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }

}
