using System;

namespace SensorManagementSystem.Models.ViewModels
{
	public class UserSensorViewModel
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public double? MinRangeValue { get; set; }

		public double? MaxRangeValue { get; set; }

		public int PollingInterval { get; set; }

		public bool IsPublic { get; set; }

		public bool? IsAlarmOn { get; set; }

		public double Longitude { get; set; }

		public double Latitude { get; set; }

		public string Value { get; set; }

		public DateTime? UpdatedOn { get; set; }
	}
}
