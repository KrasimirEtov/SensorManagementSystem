using System;

namespace SensorManagementSystem.Models.ViewModels
{
	public class UserSensorTableViewModel
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public string MeasureType { get; set; }

		public string MeasureUnit { get; set; }

		public bool IsSwitch { get; set; }

		public double? MinRangeValue { get; set; }

		public double? MaxRangeValue { get; set; }

		public int PollingInterval { get; set; }

		public bool IsPublic { get; set; }

		public bool? IsAlarmOn { get; set; }

		public string Value { get; set; }

		public DateTime? UpdatedOn { get; set; }
	}
}
