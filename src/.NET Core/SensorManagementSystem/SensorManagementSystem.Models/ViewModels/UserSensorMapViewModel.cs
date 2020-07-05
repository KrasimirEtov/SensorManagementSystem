using System;
using System.Collections.Generic;
using System.Text;

namespace SensorManagementSystem.Models.ViewModels
{
	public class UserSensorMapViewModel
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public string Name { get; set; }

		public double Longitude { get; set; }

		public double Latitude { get; set; }

		public bool IsPublic { get; set; }

		public string MeasureType { get; set; }

		public DateTime CreatedOn { get; set; }
	}
}
