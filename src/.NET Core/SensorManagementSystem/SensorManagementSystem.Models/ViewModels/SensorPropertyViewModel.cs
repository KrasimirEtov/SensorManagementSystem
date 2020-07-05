using System;

namespace SensorManagementSystem.Models.ViewModels
{
	public class SensorPropertyViewModel
	{
		public SensorPropertyViewModel()
		{

		}

		public int Id { get; set; }

		public string MeasureType { get; set; }

		public string MeasureUnit { get; set; }

		public bool IsSwitch { get; set; }

		public DateTime? CreatedOn { get; set; }

		public DateTime? ModifiedOn { get; set; }
	}
}
