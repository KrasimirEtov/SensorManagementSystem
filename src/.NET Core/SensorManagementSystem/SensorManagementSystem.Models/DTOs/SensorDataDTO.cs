using SensorManagementSystem.Models.Enums;

namespace SensorManagementSystem.Models.DTOs
{
	public class SensorDataDTO
	{
		public SensorType SensorType { get; set; }

		public int PollingInterval { get; set; }

		public double? MinRangeValue { get; set; }

		public double? MaxRangeValue { get; set; }

		public string Value { get; set; }
	}
}
