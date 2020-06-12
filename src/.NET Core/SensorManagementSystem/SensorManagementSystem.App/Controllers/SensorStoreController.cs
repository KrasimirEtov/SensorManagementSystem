using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SensorManagementSystem.Models.ViewModels;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.App.Controllers
{
	public class SensorStoreController : Controller
	{
		private readonly ISensorService _sensorService;
		private readonly ISensorPropertyService _sensorPropertyService;
		private readonly IMapper _mapper;

		public SensorStoreController(ISensorService sensorService, ISensorPropertyService sensorPropertyService, IMapper mapper)
		{
			_sensorService = sensorService;
			_sensorPropertyService = sensorPropertyService;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			List<SensorViewModel> allSensors = (await _sensorService.GetAllAsync<SensorViewModel>()).ToList();

			return View(allSensors);
		}
	}
}
