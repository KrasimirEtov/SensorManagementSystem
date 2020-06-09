using System.Collections.Generic;

namespace SensorManagementSystem.Models.Entities
{
	/// <summary>
	/// SensorProperty entity
	/// </summary>
	public class SensorPropertyEntity : BaseEntity
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SensorPropertyEntity()
		{
			Sensors = new HashSet<SensorEntity>();
		}

		/// <summary>
		/// Type of the sensor
		/// </summary>
		public string MeasureType { get; set; }

		/// <summary>
		/// Measure unit of the sensor
		/// </summary>
		public string MeasureUnit { get; set; }

		/// <summary>
		/// Is sensor a switch or not
		/// </summary>
		public bool IsSwitch { get; set; }

		/// <summary>
		/// Sensor entities
		/// </summary>
		public virtual ICollection<SensorEntity> Sensors { get; set; }
	}
}
