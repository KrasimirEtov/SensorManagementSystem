using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SensorManagementSystem.Common;
using SensorManagementSystem.Models.Entities;
using SensorManagementSystem.Models.Enums;

namespace SensorManagementSystem.Data.Seed
{
	public class DatabaseSeeder : IDatabaseSeeder
	{
		private readonly SensorManagementSystemDbContext _dbContext;

		public DatabaseSeeder(SensorManagementSystemDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task SeedAdmin()
		{
			var user = new UserEntity()
			{
				UserName = "admin@admin.com",
				NormalizedUserName = "admin@admin.com",
				Email = "admin@admin.com",
				NormalizedEmail = "admin@admin.com",
				EmailConfirmed = true,
				LockoutEnabled = false,
				SecurityStamp = Guid.NewGuid().ToString()
			};
			
			bool roleExists = await _dbContext.Roles.AnyAsync(x => x.Name == Constants.AdminRoleName);

			if (!roleExists)
			{
				var roleStore = new RoleStore<RoleEntity, SensorManagementSystemDbContext, int>(_dbContext);
				var roleEntity = new RoleEntity()
				{
					Name = Constants.AdminRoleName,
					NormalizedName = Constants.AdminRoleName
				};

				await roleStore.CreateAsync(roleEntity);
			}

			bool userExists = await _dbContext.Users.AnyAsync(x => x.Email == user.Email);

			if (!userExists)
			{
				var passwordHasher = new PasswordHasher<UserEntity>();
				user.PasswordHash = passwordHasher.HashPassword(user, "admin");
				var userStore = new UserStore<UserEntity, RoleEntity, SensorManagementSystemDbContext, int>(_dbContext);
				await userStore.CreateAsync(user);
				await userStore.AddToRoleAsync(user, Constants.AdminRoleName);
			}

			await _dbContext.SaveChangesAsync();
		}

		public async Task SeedRoles()
		{
			var roleStore = new RoleStore<RoleEntity, SensorManagementSystemDbContext, int>(_dbContext);

			bool roleExists = await _dbContext.Roles.AnyAsync(x => x.Name == Constants.UserRoleName);

			if (!roleExists)
			{
				var roleEntity = new RoleEntity()
				{
					Name = Constants.UserRoleName,
					NormalizedName = Constants.UserRoleName
				};
				await roleStore.CreateAsync(roleEntity);
			}
		}

		public async Task SeedSensorProperies()
		{
			var isTableEmpty = await _dbContext.SensorProperties.AnyAsync();

			if (!isTableEmpty)
			{
				List<SensorPropertyEntity> toAdd = new List<SensorPropertyEntity>
				{
					new SensorPropertyEntity
					{
						MeasureType = "Electric Power Consumption",
						MeasureUnit = "W",
						IsSwitch = false
					},
					new SensorPropertyEntity
					{
						MeasureType = "Humidity",
						MeasureUnit = "%",
						IsSwitch = false
					},
					new SensorPropertyEntity
					{
						MeasureType = "Noise",
						MeasureUnit = "dB",
						IsSwitch = false
					},
					new SensorPropertyEntity
					{
						MeasureType = "Switch",
						MeasureUnit = "true/false",
						IsSwitch = true
					},
					new SensorPropertyEntity
					{
						MeasureType = "Temperature",
						MeasureUnit = "°C",
						IsSwitch = false
					}
				};

				await _dbContext.AddRangeAsync(toAdd);
				await _dbContext.SaveChangesAsync();
			}
		}
	}
}
