using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorManagementSystem.Common;
using SensorManagementSystem.Common.WebClients.Contract;
using SensorManagementSystem.Models.DTOs;

namespace SensorManagementSystem.App.Areas.Admin.Controllers
{
	[Area(Constants.AdminAreaName)]
	[Authorize(Policy = Constants.AdminPolicyName, Roles = Constants.AdminRoleName)]
	public class SensorStoreController : Controller
	{
		private readonly IHttpWebClient _httpWebClient;

		public SensorStoreController(IHttpWebClient httpWebClient)
		{
			_httpWebClient = httpWebClient;
		}

		[HttpGet]
		public async Task<IActionResult> Manage()
		{
			var sensors = await _httpWebClient.GetAsync<IEnumerable<SensorDTO>>(Constants.SensorApiClientName, "api/sensor/all");

			var sensorProperties = await _httpWebClient.GetAsync<IEnumerable<SensorPropertyDTO>>(Constants.SensorApiClientName, "api/sensorproperty/all");

			return View(sensors);
		}
	}
}
