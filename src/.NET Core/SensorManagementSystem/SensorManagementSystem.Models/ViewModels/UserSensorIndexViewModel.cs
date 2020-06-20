using Microsoft.AspNetCore.Mvc.Rendering;

namespace SensorManagementSystem.Models.ViewModels
{
	public class UserSensorIndexViewModel
	{
		public string MeasureType { get; set; }

		public SelectList MeasureTypes { get; set; }

		public string IsPrivate { get; set; }

		public string IsAlarmOn { get; set; }

		public PaginatedList<UserSensorTableViewModel> UserSensors { get; set; }
	}
}
