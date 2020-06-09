using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorManagementSystem.Common;

namespace SensorManagementSystem.App.Areas.Admin.Controllers
{
	[Area(Constants.AdminAreaName)]
	[Authorize(Policy = Constants.AdminPolicyName, Roles = Constants.AdminRoleName)]
	public class SensorStoreController : Controller
	{
		[HttpGet]
		public IActionResult Manage()
		{
			return View();
		}
	}
}
