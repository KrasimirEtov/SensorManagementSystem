using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SensorManagementSystem.Models.Contract;
using SensorManagementSystem.Models.Entities;
using SensorManagementSystem.Models.Enums;

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

		public DbSet<SensorEntity> Sensors { get; set; }

		public DbSet<SensorPropertyEntity> SensorProperties { get; set; }

		public DbSet<UserSensorEntity> UserSensors { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			// TODO: Manually edit columns of migration and make longitude, latitude not nullable
			builder.Entity<UserEntity>()
				.HasMany(u => u.Sensors)
				.WithOne(us => us.User)
				.HasForeignKey(us => us.UserId)
				.OnDelete(DeleteBehavior.NoAction);

			builder.Entity<SensorEntity>()
				.HasMany(us => us.UserSensors)
				.WithOne(s => s.Sensor)
				.HasForeignKey(s => s.SensorId)
				.OnDelete(DeleteBehavior.NoAction);

			builder.Entity<SensorEntity>()
				.HasOne(sp => sp.SensorProperty)
				.WithMany(sp => sp.Sensors)
				.OnDelete(DeleteBehavior.NoAction);

			builder.Entity<SensorPropertyEntity>()
				.HasMany(s => s.Sensors)
				.WithOne(sp => sp.SensorProperty)
				.HasForeignKey(sp => sp.SensorPropertyId);

			builder.Entity<SensorPropertyEntity>()
				.HasIndex(x => new { x.MeasureType, x.MeasureUnit })
				.IsUnique(true)
				.IsClustered(false);

			base.OnModelCreating(builder);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			ApplyAuditInfoRules();

			return base.SaveChangesAsync(cancellationToken);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				IConfigurationRoot configuration = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json")
					.Build();
				string connectionString = configuration.GetConnectionString("SensorManagementSystem");
				optionsBuilder.UseSqlServer(connectionString);
			}

			base.OnConfiguring(optionsBuilder);
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
