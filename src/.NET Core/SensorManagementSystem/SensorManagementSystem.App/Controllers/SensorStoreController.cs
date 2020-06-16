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
		private const int PageSize = 4;
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
				MeasureTypes = new SelectList(measureTypes, "MeasureType", "MeasureType"),
				Sensors = PaginatedList<SensorViewModel>.Create(allSensors, 1, PageSize)
			};

			return View(model);
		}

		public async Task<IActionResult> ReloadSensorsTable(int? pageNumber, string measureType = null)
		{
			var sensors = await _sensorService.GetAllFilteredAsync<SensorViewModel>(pageNumber ?? 1, PageSize, measureType);

			return PartialView("_SensorsTable", sensors);
		}
	}
}
