using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SensorManagementSystem.Models.Contract;

namespace SensorManagementSystem.Models.Entities
{
	public abstract class BaseEntity : IAuditable, IDeletable
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime? DeletedOn { get; set; }

		public DateTime? CreatedOn { get; set; }

		public DateTime? ModifiedOn { get; set; }
	}
}
