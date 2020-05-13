using System;

namespace SensorManagementSystem.Models.Contract
{
	public interface IAuditable
	{
		DateTime? CreatedOn { get; set; }

		DateTime? ModifiedOn { get; set; }
	}
}
