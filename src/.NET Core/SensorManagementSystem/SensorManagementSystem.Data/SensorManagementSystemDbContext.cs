using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SensorManagementSystem.Models.Contract;
using SensorManagementSystem.Models.Entities;

namespace SensorManagementSystem.Data
{
	public class SensorManagementSystemDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
	{
		public SensorManagementSystemDbContext()
		{

		}

		public SensorManagementSystemDbContext(DbContextOptions<SensorManagementSystemDbContext> options) : base(options)
		{

		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			ApplyAuditInfoRules();
			ApplyDeletionRules();

			return base.SaveChangesAsync(cancellationToken);
		}

		private void ApplyDeletionRules()
		{
			var entitiesForDeletion = this.ChangeTracker.Entries()
				.Where(e => e.State == EntityState.Deleted && e.Entity is IDeletable);

			foreach (var entry in entitiesForDeletion)
			{
				var entity = (IDeletable)entry.Entity;

				entity.DeletedOn = DateTime.UtcNow;
				entity.IsDeleted = true;
				entry.State = EntityState.Modified;
			}
		}

		private void ApplyAuditInfoRules()
		{
			var newlyCreatedEntities = this.ChangeTracker
				.Entries()
				.Where(e => e.Entity is IAuditable && ((e.State == EntityState.Added) || (e.State == EntityState.Modified)));

			foreach (var entry in newlyCreatedEntities)
			{
				var entity = (IAuditable)entry.Entity;

				if (entry.State == EntityState.Added && entity.CreatedOn == null)
				{
					entity.CreatedOn = DateTime.UtcNow;
				}
				else
				{
					entity.ModifiedOn = DateTime.UtcNow;
				}
			}
		}

	}
}
