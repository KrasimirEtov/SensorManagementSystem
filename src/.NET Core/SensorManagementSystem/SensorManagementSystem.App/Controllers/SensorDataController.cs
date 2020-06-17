using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.App.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SensorDataController : ControllerBase
	{
        private readonly ILogger<SensorDataController> _logger;
        private readonly ISensorDataService _sensorDataService;

        public SensorDataController(ILogger<SensorDataController> logger, ISensorDataService sensorDataService)
        {
            this._logger = logger;
            this._sensorDataService = sensorDataService;
        }

        [Route("all")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sensorDataDTOs = await this._sensorDataService
                .GetAllAsync();

            if (sensorDataDTOs.Any())
            {
                return Ok(sensorDataDTOs);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
