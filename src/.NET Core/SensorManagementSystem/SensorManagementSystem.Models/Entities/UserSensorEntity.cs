using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SensorManagementSystem.Models.Entities
{
	/// <summary>
	/// UserSensor entity
	/// </summary>
	public class UserSensorEntity : BaseEntity
	{
		/// <summary>
		/// User identificator. Foreign key
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// User navigation property
		/// </summary>
		public UserEntity User { get; set; }

		/// <summary>
		/// Sensor identificator. Foreign key
		/// </summary>
		public int SensorId { get; set; }

		/// <summary>
		/// Sensor navigation property
		/// </summary>
		public SensorEntity Sensor { get; set; }

		/// <summary>
		/// Name of user sensor
		/// </summary>
		[Required]
		[MaxLength(255)]
		public string Name { get; set; }

		/// <summary>
		/// Description of user sensor
		/// </summary>
		[MaxLength(255)]
		public string Description { get; set; }

		/// <summary>
		/// Minimum value that user choosed for the sensor
		/// </summary>
		public decimal? MinRangeValue { get; set; }

		/// <summary>
		/// Maximum value that user choosed for the sensor
		/// </summary>
		public decimal? MaxRangeValue { get; set; }

		/// <summary>
		/// Polling interval in seconds that user choosed for the sensor
		/// </summary>
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Polling interval must be a positive value!")]
		public int PollingInterval { get; set; }

		/// <summary>
		/// Whether sensor is publicly available or it is private
		/// </summary>
		[Required]
		public bool IsPublic { get; set; }

		/// <summary>
		/// Whether sensor alarm is turned on or off
		/// </summary>
		public bool? IsAlarmOn { get; set; }

		/// <summary>
		/// Coordinates of the sensor to appear on the map
		/// </summary>
		[Required]
		public Coordinates Coordinates { get; set; }

		/// <summary>
		/// Sensor's latest update date
		/// </summary>
		public DateTime? LastUpdatedOn { get; set; }

		/// <summary>
		/// Latest value from the sensor's api
		/// </summary>
		public string Value { get; set; }
	}

	[Owned]
	public class Coordinates
	{
		[Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180!")]
		public double Longitude { get; set; }

		[Range(-90, 90, ErrorMessage = "Longitude must be between -90 and 90!")]
		public double Latitude { get; set; }
	}
}
