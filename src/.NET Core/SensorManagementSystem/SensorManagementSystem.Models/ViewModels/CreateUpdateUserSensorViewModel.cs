using System;
using System.ComponentModel.DataAnnotations;

namespace SensorManagementSystem.Models.ViewModels
{
	public class CreateUpdateUserSensorViewModel
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public int SensorId { get; set; }

		public string MeasureType { get; set; }

		public string MeasureUnit { get; set; }

		public bool IsSwitch { get; set; }

		[Required]
		[StringLength(30, ErrorMessage = "Sensor name should be between 3 and 30 symbols.", MinimumLength = 3)]
		public string Name { get; set; }

		[StringLength(100, ErrorMessage = "Sensor description should be between 3 and 100 symbols.", MinimumLength = 3)]
		public string Description { get; set; }

		public double? CustomMinRangeValue { get; set; }

		public double? CustomMaxRangeValue { get; set; }

		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Polling interval must be a positive value!")]
		public int CustomPollingInterval { get; set; }

		public double? SensorMinRangeValue { get; set; }

		public double? SensorMaxRangeValue { get; set; }

		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Polling interval must be a positive value!")]
		public int SensorPollingInterval { get; set; }

		[Required]
		public bool IsPublic { get; set; }

		public bool? IsAlarmOn { get; set; }

		[Required]
		[Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180!")]
		public double Longitude { get; set; }

		[Required]
		[Range(-90, 90, ErrorMessage = "Longitude must be between -90 and 90!")]
		public double Latitude { get; set; }

		public string Value { get; set; }

		public DateTime? UpdatedOn { get; set; }
	}
}
