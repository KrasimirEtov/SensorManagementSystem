using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SensorManagementSystem.Models.ViewModels
{
	public class SensorStoreViewModel
	{
		public string MeasureType { get; set; }

		public SelectList MeasureTypes { get; set; }

		public List<SensorViewModel> Sensors { get; set; }
	}
}
