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

		[Required]
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

		public bool IsPublic { get; set; }

		public bool IsAlarmOn { get; set; }

		[Required(ErrorMessage = "Latitude is required!")]
		public double Longitude { get; set; }

		[Required(ErrorMessage = "Longitude is required and !")]
		public double Latitude { get; set; }
	}
}
