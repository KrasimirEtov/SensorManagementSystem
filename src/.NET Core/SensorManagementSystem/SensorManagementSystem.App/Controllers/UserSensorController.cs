using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SensorManagementSystem.Models.DTOs;
using SensorManagementSystem.Models.ViewModels;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.App.Controllers
{
	[Authorize]
	public class UserSensorController : Controller
	{
		private const int PageSize = 3;
		private const int DefaultPageIndex = 1;
		private readonly ISensorService _sensorService;
		private readonly ISensorPropertyService _sensorPropertyService;
		private readonly IUserSensorService _userSensorService;
		private readonly IMapper _mapper;

		public UserSensorController(ISensorService sensorService, ISensorPropertyService sensorPropertyService, IUserSensorService userSensorService, IMapper mapper)
		{
			_sensorService = sensorService;
			_sensorPropertyService = sensorPropertyService;
			_userSensorService = userSensorService;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var userSensors = await _userSensorService.GetAllByUserIdAsync<UserSensorViewModel>(userId);
			var measureTypes = await _sensorPropertyService.GetAllAsync<SensorPropertyViewModel>();
			var model = GetUserSensorIndexViewModel(measureTypes, userSensors);

			return View(model);
		}

		public async Task<IActionResult> ReloadUserSensorsTable([FromQuery]int pageIndex, string measureType = null, bool? isPublic = null, bool? isAlarmOn = null, string searchTerm = null)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var model = await _userSensorService
				.GetAllFilteredAsync<UserSensorTableViewModel>(userId, pageIndex, PageSize, measureType, isPublic, isAlarmOn, searchTerm);

			return PartialView("_UserSensorsTable", model);
		}

		[HttpGet]
		public async Task<IActionResult> Create(int sensorId)
		{
			var sensor = await _sensorService.GetByIdAsync<SensorDTO>(sensorId);
			var sensorProperty = await _sensorPropertyService.GetByIdAsync<SensorPropertyDTO>(sensor.SensorPropertyId);

			var model = GetCreateUpdateSensorViewModel(sensor, sensorProperty);

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateUpdateUserSensorViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _userSensorService.CreateAsync(model);
			}

			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var model = await _userSensorService.GetAsync<CreateUpdateUserSensorViewModel>(id);

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(CreateUpdateUserSensorViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _userSensorService.UpdateAsync(model);
			}

			return RedirectToAction("Index");
		}

		private CreateUpdateUserSensorViewModel GetCreateUpdateSensorViewModel(SensorDTO sensor, SensorPropertyDTO sensorProperty)
		{
			return new CreateUpdateUserSensorViewModel
			{
				SensorId = sensor.Id,
				SensorPollingInterval = sensor.PollingInterval,
				SensorMaxRangeValue = sensor.MaxRangeValue,
				SensorMinRangeValue = sensor.MinRangeValue,
				MeasureType = sensorProperty.MeasureType,
				MeasureUnit = sensorProperty.MeasureUnit,
				IsSwitch = sensorProperty.IsSwitch,
				UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
			};
		}

		private UserSensorIndexViewModel GetUserSensorIndexViewModel(IEnumerable<SensorPropertyViewModel> sensorPropertyViewModels, IEnumerable<UserSensorViewModel> userSensorViewModels)
		{
			var userSensorTableViewModels = GetUserSensorTableViewModel(userSensorViewModels, sensorPropertyViewModels);
			var minPollingInterval = userSensorTableViewModels.Min(x => x.PollingInterval);
			return new UserSensorIndexViewModel
			{
				MeasureTypes = new SelectList(sensorPropertyViewModels, "MeasureType", "MeasureType"),
				UserSensors = PaginatedList<UserSensorTableViewModel>.Create(userSensorTableViewModels, DefaultPageIndex, PageSize, minPollingInterval)
			};
		}

		private IEnumerable<UserSensorTableViewModel> GetUserSensorTableViewModel(IEnumerable<UserSensorViewModel> userSensorViewModels, IEnumerable<SensorPropertyViewModel> sensorPropertyViewModels)
		{
			var result = new List<UserSensorTableViewModel>();

			foreach (var item in userSensorViewModels)
			{
				var sensorProperty = sensorPropertyViewModels.FirstOrDefault(x => x.Id == item.SensorPropertyId);

				var itemToAdd = new UserSensorTableViewModel
				{
					IsAlarmOn = item.IsAlarmOn,
					Description = item.Description,
					IsPublic = item.IsPublic,
					MaxRangeValue = item.MaxRangeValue,
					MinRangeValue = item.MinRangeValue,
					Name = item.Name,
					PollingInterval = item.PollingInterval,
					UpdatedOn = item.UpdatedOn,
					Value = string.Format("{0:0.00}", double.Parse(item.Value)),
					IsSwitch = sensorProperty.IsSwitch,
					MeasureType = sensorProperty.MeasureType,
					MeasureUnit = sensorProperty.MeasureUnit
				};

				result.Add(itemToAdd);
			}

			return result;
		}
	}
}
