using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SensorManagementSystem.Models.Enums;

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
		//[JsonConverter(typeof(StringEnumConverter), true)]
		public SensorType Type { get; set; }

		/// <summary>
		/// Measure unit of the sensor
		/// </summary>
		public string MeasureUnit { get; set; }

		/// <summary>
		/// Sensor entities
		/// </summary>
		public virtual ICollection<SensorEntity> Sensors { get; set; }
	}
}
