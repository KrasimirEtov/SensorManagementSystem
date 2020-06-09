using System;
using SensorManagementSystem.Models.Contract;

namespace SensorManagementSystem.Models.DTOs
{
	public class SensorPropertyDTO : IAuditable
	{
		public int Id { get; set; }

		public string MeasureType { get; set; }

		public string MeasureUnit { get; set; }

		public bool IsSwitch { get; set; }

		public DateTime? CreatedOn { get; set; }

		public DateTime? ModifiedOn { get; set; }
	}
}
