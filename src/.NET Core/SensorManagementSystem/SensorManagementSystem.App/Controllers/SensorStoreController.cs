using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SensorManagementSystem.Common;
using SensorManagementSystem.Models.ViewModels;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.App.Controllers
{
	public class SensorStoreController : Controller
	{
		private readonly ISensorService _sensorService;
		private readonly ISensorPropertyService _sensorPropertyService;
		private readonly ICachingService _cachingService;
		private readonly IMapper _mapper;

		public SensorStoreController(ISensorService sensorService, ISensorPropertyService sensorPropertyService, ICachingService cachingService, IMapper mapper)
		{
			_sensorService = sensorService;
			_sensorPropertyService = sensorPropertyService;
			_cachingService = cachingService;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			var measureTypes = await _sensorPropertyService.GetAllAsync<SensorPropertyViewModel>();
			var allSensors = (await _sensorService.GetAllAsync<SensorViewModel>()).ToList();

			var model = new SensorStoreViewModel
			{
				MeasureTypes = new SelectList(measureTypes, "Id", "MeasureType"),
				Sensors = allSensors
			};

			return View(model);
		}

		public async Task<IActionResult> ReloadSensorsTable(string measureType = null)
		{
			var sensors = await _sensorService.GetAllAsync<SensorViewModel>(measureType);

			return PartialView("_SensorsTable", sensors);
		}
	}
}
