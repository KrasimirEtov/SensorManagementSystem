using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private readonly ILogger<SensorDataController> logger;
        private readonly ISensorDataService sensorDataService;

        public SensorDataController(ILogger<SensorDataController> logger, ISensorDataService sensorDataService)
        {
            this.logger = logger;
            this.sensorDataService = sensorDataService;
        }

        [Route("all")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sensorDataDTOs = await this.sensorDataService
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