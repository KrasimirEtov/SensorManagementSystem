using System;
using SensorManagementSystem.Models.Contract;
using SensorManagementSystem.Models.Enums;

namespace SensorManagementSystem.Models.DTOs
{
	public class SensorPropertyDTO
	{
		public int Id { get; set; }

		public SensorType Type { get; set; }

		public string MeasureUnit { get; set; }

		public DateTime? CreatedOn { get; set; }

		public DateTime? ModifiedOn { get; set; }

		public bool? IsDeleted { get; set; }

		public DateTime? DeletedOn { get; set; }
	}
}
