using System;
using System.Collections.Generic;
using System.Text;

namespace SensorManagementSystem.Models.ViewModels
{
	public class HomeIndexViewModel
	{
		public int UsersCount { get; set; }

		public int SensorsInStoreCount { get; set; }

		public int MeasureTypesCount { get; set; }

		public int PublicUserSensorsCount { get; set; }

		public int? AuthenticatedUserSensorsCount { get; set; }
	}
}
