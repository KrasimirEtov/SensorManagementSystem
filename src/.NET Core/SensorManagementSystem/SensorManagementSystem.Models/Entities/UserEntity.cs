using System;
using Microsoft.AspNetCore.Identity;
using SensorManagementSystem.Models.Contract;

namespace SensorManagementSystem.Models.Entities
{
	public class UserEntity : IdentityUser<int>, IAuditable
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public int Age { get; set; }

		public DateTime? CreatedOn { get; set; }

		public DateTime? ModifiedOn { get; set; }
	}
}
