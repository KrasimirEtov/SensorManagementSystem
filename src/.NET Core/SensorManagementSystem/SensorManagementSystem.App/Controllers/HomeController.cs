using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SensorManagementSystem.Models.Entities;
using SensorManagementSystem.Models.ViewModels;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.App.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ISensorService _sensorService;
		private readonly ISensorPropertyService _sensorPropertyService;
		private readonly IUserSensorService _userSensorService;
		private readonly UserManager<UserEntity> _userManager;

		public HomeController(ILogger<HomeController> logger, ISensorService sensorService, ISensorPropertyService sensorPropertyService, IUserSensorService userSensorService, UserManager<UserEntity> userManager)
		{
			_logger = logger;
			_sensorService = sensorService;
			_sensorPropertyService = sensorPropertyService;
			_userSensorService = userSensorService;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var usersCount = await _userManager.Users.CountAsync();
			var sensorsInStoreCount = await _sensorService.GetCountAsync();
			var measureTypesCount = await _sensorPropertyService.GetCountAsync();
			var publicUserSensorsCount = await _userSensorService.GetCountAsync();

			var model = new HomeIndexViewModel
			{
				UsersCount = usersCount,
				SensorsInStoreCount = sensorsInStoreCount,
				MeasureTypesCount = measureTypesCount,
				PublicUserSensorsCount = publicUserSensorsCount,
				AuthenticatedUserSensorsCount = User.Identity.IsAuthenticated ? await _userSensorService.GetCountByUserIdAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))) : (int?)null
			};

			return View(model);
		}
	}
}
