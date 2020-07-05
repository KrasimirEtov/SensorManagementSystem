using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SensorManagementSystem.Models.ViewModels;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.App.Controllers
{
	public class SensorStoreController : Controller
	{
		private readonly ISensorService _sensorService;
		private readonly ISensorPropertyService _sensorPropertyService;

		public SensorStoreController(ISensorService sensorService, ISensorPropertyService sensorPropertyService)
		{
			_sensorService = sensorService;
			_sensorPropertyService = sensorPropertyService;
		}

		public async Task<IActionResult> Index()
		{
			var measureTypes = await _sensorPropertyService.GetAllAsync<SensorPropertyViewModel>();
			var allSensors = (await _sensorService.GetAllAsync<SensorViewModel>()).ToList();

			var model = new SensorStoreViewModel
			{
				MeasureTypes = new SelectList(measureTypes, "MeasureType", "MeasureType"),
				Sensors = allSensors
			};

			return View(model);
		}

		public async Task<IActionResult> ReloadSensorsTable(string measureType = null)
		{
			var sensors = (await _sensorService.GetAllFilteredAsync<SensorViewModel>(measureType)).ToList();

			return PartialView("_SensorsTable", sensors);
		}
	}
}
