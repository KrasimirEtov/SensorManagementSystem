using Microsoft.AspNetCore.Mvc.Rendering;

namespace SensorManagementSystem.Models.ViewModels
{
	public class SensorStoreViewModel
	{
		public string MeasureType { get; set; }

		public SelectList MeasureTypes { get; set; }

		public PaginatedList<SensorViewModel> Sensors { get; set; }
	}
}
