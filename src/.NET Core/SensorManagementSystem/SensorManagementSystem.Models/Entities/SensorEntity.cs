using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SensorManagementSystem.Models.Entities
{
	/// <summary>
	/// Sensor entity
	/// </summary>
	public class SensorEntity : BaseEntity
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SensorEntity()
		{
			UserSensors = new HashSet<UserSensorEntity>();
		}

		/// <summary>
		/// Sensor property ID. Foreign key
		/// </summary>
		public int SensorPropertyId { get; set; }

		/// <summary>
		/// Navigation property for SensorProperty entity
		/// </summary>
		public SensorPropertyEntity SensorProperty { get; set; }

		/// <summary>
		/// Sensor's description
		/// </summary>	
		[Required]
		[MaxLength(255)]
		public string Description { get; set; }

		/// <summary>
		/// Polling interval in seconds
		/// </summary>
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Polling interval must be a positive value!")]
		public int PollingInterval { get; set; }

		/// <summary>
		/// Minimum value that sensor provides
		/// </summary>
		public double? MinRangeValue { get; set; }

		/// <summary>
		/// Maximum value that sensor provides
		/// </summary>
		public double? MaxRangeValue { get; set; }

		/// <summary>
		/// User sensor entities
		/// </summary>
		public virtual ICollection<UserSensorEntity> UserSensors { get; set; }
	}
}
