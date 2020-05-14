using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SensorManagementSystem.Models.DTOs;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ILogger<SensorController> logger;
        private readonly ISensorService sensorService;

        public SensorController(ILogger<SensorController> logger, ISensorService sensorService)
        {
            this.logger = logger;
            this.sensorService = sensorService;
        }

        [Route("all")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allSensors = await this.sensorService.GetAllAsync();

            return Ok(allSensors);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var sensor = await this.sensorService.GetByIdAsync(id);

            return Ok(sensor);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SensorDTO payload)
        {
            // TODO: Add validation
            await this.sensorService.CreateAsync(payload);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]SensorDTO payload)
        {
            // TODO: Add validation
            await this.sensorService.UpdateAsync(payload);

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await this.sensorService.DeleteAsync(id);

            return Ok();
        }
    }
}