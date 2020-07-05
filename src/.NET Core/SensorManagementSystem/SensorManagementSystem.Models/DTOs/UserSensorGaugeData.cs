using System;
using System.Collections.Generic;
using System.Text;

namespace SensorManagementSystem.Models.DTOs
{
	public class UserSensorGaugeData
	{
		public string Name { get; set; }

		public string Value { get; set; }

		public double? MinRange { get; set; }

		public double? MaxRange { get; set; }
	}
}
