using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SensorManagementSystem.Common.Extensions;
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

		public UserSensorController(ISensorService sensorService, ISensorPropertyService sensorPropertyService, IUserSensorService userSensorService)
		{
			_sensorService = sensorService;
			_sensorPropertyService = sensorPropertyService;
			_userSensorService = userSensorService;
		}

		public async Task<IActionResult> Index()
		{
			var userId = User.GetId();
			var measureTypes = await _sensorPropertyService.GetAllAsync<SensorPropertyViewModel>();
			var model = await GetUserSensorIndexViewModel(userId, measureTypes);
			return View(model);
		}

		public async Task<IActionResult> ReloadUserSensorsTable([FromQuery] int pageIndex, string measureType = null, bool? isPublic = null, bool? isAlarmOn = null, string searchTerm = null)
		{
			var userId = User.GetId();
			var model = await _userSensorService
				.GetAllFilteredAsync<UserSensorTableViewModel>(userId, pageIndex, PageSize, measureType, isPublic, isAlarmOn, searchTerm);

			return PartialView("_UserSensorsTable", model);
		}

		[HttpGet]
		public async Task<IActionResult> Create(int sensorId)
		{
			var sensor = await _sensorService.GetByIdAsync<SensorDTO>(sensorId);

			if (sensor == null)
			{
				return RedirectToAction("PageNotFound", "Error");
			}

			var sensorProperty = await _sensorPropertyService.GetByIdAsync<SensorPropertyDTO>(sensor.SensorPropertyId);

			var model = GetCreateUpdateSensorViewModel(sensor, sensorProperty);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
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
			var userId = User.GetId();

			if (model.UserId != userId)
			{
				return RedirectToAction("PageNotFound", "Error");
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(CreateUpdateUserSensorViewModel model)
		{
			var userId = User.GetId();

			if (model.UserId != userId)
			{
				return RedirectToAction("PageNotFound", "Error");
			}

			if (ModelState.IsValid)
			{
				await _userSensorService.UpdateAsync(model);
			}

			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> GetGaugeData(int userSensorId)
		{
			var sensorValue = await _userSensorService.GetGaugeDataAsync(userSensorId);

			return Json(sensorValue);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var userId = User.GetId();
			var sensorEntity = await _userSensorService.GetAsync<UserSensorViewModel>(id);

			if (sensorEntity.UserId != userId)
			{
				return RedirectToAction("PageNotFound", "Error");
			}

			await _userSensorService.DeleteAsync(id);

			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<JsonResult> GetUserSensorCoordinates()
		{
			int userId = User.GetId();
			var sensors = await _userSensorService.GetAllByUserIdAsync<UserSensorMapViewModel>(userId);

			return Json(sensors);
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

		private async Task<UserSensorIndexViewModel> GetUserSensorIndexViewModel(int userId, IEnumerable<SensorPropertyViewModel> sensorPropertyViewModels)
		{
			var userSensorTableViewModels = await _userSensorService.GetAllFilteredAsync<UserSensorTableViewModel>(userId, DefaultPageIndex, PageSize);

			return new UserSensorIndexViewModel
			{
				MeasureTypes = new SelectList(sensorPropertyViewModels, "MeasureType", "MeasureType"),
				UserSensors = userSensorTableViewModels
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
					IsSwitch = sensorProperty.IsSwitch,
					MeasureType = sensorProperty.MeasureType,
					MeasureUnit = sensorProperty.MeasureUnit,
					Id = item.Id
				};

				if (double.TryParse(item.Value, out double parsedDoubleValue))
				{
					itemToAdd.Value = string.Format("{0:0.00}", parsedDoubleValue);
				}
				else if (bool.TryParse(item.Value, out bool parsedBoolSensorValue))
				{
					itemToAdd.Value = parsedBoolSensorValue ? 1.ToString() : 0.ToString();
				}
				else
				{
					itemToAdd.Value = null;
				}

				result.Add(itemToAdd);
			}

			return result;
		}
	}
}
