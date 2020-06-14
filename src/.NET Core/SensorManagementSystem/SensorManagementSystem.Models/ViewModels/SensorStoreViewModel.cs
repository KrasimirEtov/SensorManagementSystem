using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SensorManagementSystem.Models.ViewModels
{
	public class SensorStoreViewModel
	{
		public int MeasureTypeId { get; set; }

		public SelectList MeasureTypes { get; set; }

		public IEnumerable<SensorViewModel> Sensors { get; set; }
	}
}
