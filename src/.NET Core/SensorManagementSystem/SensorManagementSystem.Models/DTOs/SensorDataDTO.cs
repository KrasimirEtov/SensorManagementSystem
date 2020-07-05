namespace SensorManagementSystem.Models.DTOs
{
	public class SensorDataDTO
	{
		public string MeasureType { get; set; }

		public bool IsSwitch { get; set; }

		public int PollingInterval { get; set; }

		public double? MinRangeValue { get; set; }

		public double? MaxRangeValue { get; set; }

		public string Value { get; set; }
	}
}
