using System;
using SensorManagementSystem.Models.Contract;

namespace SensorManagementSystem.Models.Entities
{
	public abstract class BaseEntity : IAuditable, IDeletable
	{
		// TODOKRASI: Add this to fluent api
		//[Key]
		//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime? DeletedOn { get; set; }

		public DateTime? CreatedOn { get; set; }

		public DateTime? ModifiedOn { get; set; }
	}
}
