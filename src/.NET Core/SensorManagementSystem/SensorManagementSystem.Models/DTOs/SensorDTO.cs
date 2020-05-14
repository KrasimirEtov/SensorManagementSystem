using System;
using System.ComponentModel.DataAnnotations;
using SensorManagementSystem.Models.Contract;

namespace SensorManagementSystem.Models.DTOs
{
	public class SensorDTO
	{
		public int Id { get; set; }

		public SensorPropertyDTO SensorProperty { get; set; }

		[MaxLength(255)]
		public string Description { get; set; }

		public decimal? MinRangeValue { get; set; }

		public decimal? MaxRangeValue { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "Polling interval must be a positive value!")]
		public int PollingInterval { get; set; }

		public DateTime? CreatedOn { get; set; }

		public DateTime? ModifiedOn { get; set; }

		public bool? IsDeleted { get; set; }

		public DateTime? DeletedOn { get; set; }
	}
}
