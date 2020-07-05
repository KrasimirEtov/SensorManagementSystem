using System;
using System.ComponentModel.DataAnnotations;
using SensorManagementSystem.Models.Contract;

namespace SensorManagementSystem.Models.DTOs
{
	public class SensorDTO : IAuditable
	{
		public int Id { get; set; }

		public int SensorPropertyId { get; set; }

		[MaxLength(255)]
		public string Description { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "Polling interval must be a positive value!")]
		public int PollingInterval { get; set; }

		public double? MinRangeValue { get; set; }

		public double? MaxRangeValue { get; set; }

		public DateTime? CreatedOn { get; set; }

		public DateTime? ModifiedOn { get; set; }
	}
}
