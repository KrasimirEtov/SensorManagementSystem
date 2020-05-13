using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SensorManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ILogger<SensorController> logger;

        public SensorController(ILogger<SensorController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}