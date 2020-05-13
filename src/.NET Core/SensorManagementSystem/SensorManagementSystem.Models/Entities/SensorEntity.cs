namespace SensorManagementSystem.Models.Entities
{
	/// <summary>
	/// Sensor entity
	/// </summary>
	public class SensorEntity : BaseEntity
	{
		/// <summary>
		/// Sensor's description
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Minimum value that sensor provides
		/// </summary>
		public decimal MinRangeValue { get; set; }

		/// <summary>
		/// Maximum value that sensor provides
		/// </summary>
		public decimal MaxRangeValue { get; set; }

		/// <summary>
		/// Polling interval in seconds
		/// </summary>
		public int PollingInterval { get; set; }

		/// <summary>
		/// Sensor property ID. Foreign key
		/// </summary>
		public int SensorPropertyId { get; set; }

		/// <summary>
		/// Navigation property for SensorProperty entity
		/// </summary>
		public SensorPropertyEntity SensorProperty { get; set; }
	}
}
