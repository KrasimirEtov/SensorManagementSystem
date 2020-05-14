using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SensorManagementSystem.Models.DTOs;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorPropertyController : ControllerBase
    {
        private readonly ILogger<SensorPropertyController> logger;
        private readonly ISensorPropertyService sensorPropertyService;

        public SensorPropertyController(ILogger<SensorPropertyController> logger, ISensorPropertyService sensorPropertyService)
        {
            this.logger = logger;
            this.sensorPropertyService = sensorPropertyService;
        }

        [Route("all")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allSensors = await this.sensorPropertyService.GetAllAsync();

            return Ok(allSensors);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var sensor = await this.sensorPropertyService.GetByIdAsync(id);

            return Ok(sensor);
        }

        [Route("sensortypes")]
        [HttpGet]
        public async Task<IActionResult> GetSensorTypes([FromQuery]bool useFilter = false)
        {
            var sensorTypes = await this.sensorPropertyService.GetSensorTypesAsync(useFilter);

            return Ok(sensorTypes);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SensorPropertyDTO payload)
        {
            // TODO: Add validation
            await this.sensorPropertyService.CreateAsync(payload);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]SensorPropertyDTO payload)
        {
            // TODO: Add validation
            await this.sensorPropertyService.UpdateAsync(payload);

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await this.sensorPropertyService.DeleteAsync(id);

            return Ok();
        }
    }
}