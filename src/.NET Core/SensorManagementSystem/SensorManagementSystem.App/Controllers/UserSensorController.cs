using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorManagementSystem.Models.DTOs;
using SensorManagementSystem.Models.ViewModels;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.App.Controllers
{
	[Authorize]
	public class UserSensorController : Controller
	{
		private readonly ISensorService _sensorService;
		private readonly ISensorPropertyService _sensorPropertyService;
		private readonly IMapper _mapper;

		public UserSensorController(ISensorService sensorService, ISensorPropertyService sensorPropertyService, IMapper mapper)
		{
			_sensorService = sensorService;
			_sensorPropertyService = sensorPropertyService;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Create(int sensorId)
		{
			var sensor = await _sensorService.GetByIdAsync<SensorDTO>(sensorId);
			var sensorProperty = await _sensorPropertyService.GetByIdAsync<SensorPropertyDTO>(sensor.SensorPropertyId);
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			var model = GetViewModel(sensor, sensorProperty, userId);

			return View(model);
		}

		private CreateUpdateUserSensorViewModel GetViewModel(SensorDTO sensor, SensorPropertyDTO sensorProperty, int userId)
		{
			return new CreateUpdateUserSensorViewModel
			{
				SensorId = sensor.Id,
				SensorPollingInterval = sensor.PollingInterval,
				SensorMaxRangeValue = sensor.MaxRangeValue,
				SensorMinRangeValue = sensor.MinRangeValue,
				MeasureType = sensorProperty.MeasureType,
				MeasureUnit = sensorProperty.MeasureUnit,
				IsSwitch = sensorProperty.IsSwitch
			};
		}
	}
}
