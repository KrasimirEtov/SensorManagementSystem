using System;
using System.ComponentModel.DataAnnotations;

namespace SensorManagementSystem.Models.ViewModels
{
	public class SensorViewModel
	{
		public SensorViewModel()
		{

		}

		public int Id { get; set; }

		[MaxLength(255)]
		public string Description { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "Polling interval must be a positive value!")]
		public int PollingInterval { get; set; }

		public double? MinRangeValue { get; set; }

		public double? MaxRangeValue { get; set; }

		public SensorPropertyViewModel SensorProperty { get; set; }
	}
}
