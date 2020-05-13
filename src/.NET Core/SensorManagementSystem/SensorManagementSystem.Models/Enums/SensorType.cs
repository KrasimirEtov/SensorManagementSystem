using System.ComponentModel;

namespace SensorManagementSystem.Models.Enums
{
	public enum SensorType
	{
		[Description("Temperature")]
		Temperature,
		[Description("Noise")]
		Noise,
		[Description("Humidity")]
		Humidity,
		[Description("Switch")]
		Switch,
		[Description("Electric Power Consumption")]
		ElectricPowerConsumption
	}
}
