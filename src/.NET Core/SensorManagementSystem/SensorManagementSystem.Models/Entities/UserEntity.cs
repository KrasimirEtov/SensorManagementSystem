using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SensorManagementSystem.Models.Contract;

namespace SensorManagementSystem.Models.Entities
{
	/// <summary>
	/// User entity
	/// </summary>
	public class UserEntity : IdentityUser<int>, IAuditable
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public UserEntity()
		{
			Sensors = new HashSet<UserSensorEntity>();
		}

		/// <summary>
		/// Registration date
		/// </summary>
		public DateTime? CreatedOn { get; set; }

		/// <summary>
		/// Last setting modified on date
		/// </summary>
		public DateTime? ModifiedOn { get; set; }

		/// <summary>
		/// User sensors
		/// </summary>
		public ICollection<UserSensorEntity> Sensors { get; set; }
	}
}
