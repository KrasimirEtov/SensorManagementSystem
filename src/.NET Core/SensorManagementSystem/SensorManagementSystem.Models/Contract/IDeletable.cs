using System;

namespace SensorManagementSystem.Models.Contract
{
	public interface IDeletable
	{
		bool IsDeleted { get; set; }

		DateTime? DeletedOn { get; set; }
	}
}
