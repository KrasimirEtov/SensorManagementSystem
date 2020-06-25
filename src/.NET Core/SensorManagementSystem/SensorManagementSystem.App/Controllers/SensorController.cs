using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SensorManagementSystem.App.Hubs.Contract;
using SensorManagementSystem.Models.DTOs;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.App.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SensorController : ControllerBase
	{
        private readonly ILogger<SensorController> _logger;
        private readonly ISensorService _sensorService;
		private readonly INotificationManager _notificationManager;

		public SensorController(ILogger<SensorController> logger, ISensorService sensorService, INotificationManager notificationManager)
        {
            this._logger = logger;
            this._sensorService = sensorService;
			this._notificationManager = notificationManager;
		}

        [Route("all")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allSensors = await this._sensorService.GetAllAsync<SensorDTO>();

            return Ok(allSensors);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var sensor = await this._sensorService.GetByIdAsync<SensorDTO>(id);

            return Ok(sensor);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SensorDTO payload)
        {
            var result = ValidateDTO(payload);

            if (!string.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }

            await this._sensorService.CreateAsync(payload);
            await _notificationManager.SendToAuthenticatedUsersAsync("New sensor has been added to the store!");

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SensorDTO payload)
        {
            var result = ValidateDTO(payload);

            if (!string.IsNullOrEmpty(result))
			{
                return BadRequest(result);
			}

            await this._sensorService.UpdateAsync(payload);

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await this._sensorService.DeleteAsync(id);

            return Ok();
        }

        private string ValidateDTO(SensorDTO payload)
		{
            string errorMessage = string.Empty;

            if (payload.MinRangeValue.HasValue && payload.MaxRangeValue.HasValue)
			{
                if (payload.MinRangeValue.Value > payload.MaxRangeValue.Value)
				{
                    errorMessage += "Min Range shold be less than Max Range!\n";
				}
                if (payload.MaxRangeValue.Value < payload.MinRangeValue.Value)
				{
                    errorMessage += "Max Range should be more than Min Range!\n";
				}
                if (payload.PollingInterval <= 0)
				{
                    errorMessage += "Polling Interval should be a positive value!";
				}
			}

            return errorMessage;
		}
    }
}
