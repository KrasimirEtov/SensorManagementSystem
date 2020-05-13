using SensorManagementSystem.Models.Enums;

namespace SensorManagementSystem.Models.Entities
{
	/// <summary>
	/// SensorProperty entity
	/// </summary>
	public class SensorPropertyEntity : BaseEntity
	{
		/// <summary>
		/// Type of the sensor
		/// </summary>
		public SensorType Type { get; set; }

		/// <summary>
		/// Measure unit of the sensor
		/// </summary>
		public string MeasureUnit { get; set; }
	}
}
