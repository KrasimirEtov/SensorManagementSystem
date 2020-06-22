﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SensorManagementSystem.Data;
using SensorManagementSystem.Models.Entities;
using SensorManagementSystem.Models.ViewModels;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.Services
{
	public class UserSensorService : IUserSensorService
	{
		private readonly SensorManagementSystemDbContext _dbContext;
		private readonly IMapper _mapper;

		public UserSensorService(SensorManagementSystemDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task CreateAsync<T>(T model)
		{
			var dbEntity = MapToEntity(model);

			if (!dbEntity.IsAlarmOn.HasValue || (dbEntity.IsAlarmOn.HasValue && !dbEntity.IsAlarmOn.Value))
			{
				dbEntity.MinRangeValue = null;
				dbEntity.MaxRangeValue = null;
			}

			await _dbContext.UserSensors
				.AddAsync(dbEntity);

			await _dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<T>> GetAllByUserIdAsync<T>(int userId)
		{
			var userSensorEntities = await _dbContext.UserSensors
				.Include(x => x.Sensor)
				.ThenInclude(x => x.SensorProperty)
				.Where(x => x.UserId == userId)
				.ToListAsync();

			return MapToViewModel<T>(userSensorEntities);
		}

		public async Task<PaginatedList<T>> GetAllFilteredAsync<T>(int userId, int pageIndex, int pageSize, string measureType = null, bool? isPublic = null, bool? isAlarmOn = null, string searchTerm = null)
		{
			var userSensorEntitiesQuery = this._dbContext.UserSensors
				.Include(x => x.Sensor)
				.ThenInclude(x => x.SensorProperty)
				.Where(x => x.UserId == userId)
				.AsQueryable();

			userSensorEntitiesQuery = GetFilteredQuery(userSensorEntitiesQuery, measureType, isPublic, isAlarmOn, searchTerm);

			var filteredUserSensorEntities = await userSensorEntitiesQuery.ToListAsync();
			var minPollingInterval = filteredUserSensorEntities.Min(x => x.PollingInterval);
			return PaginatedList<T>.Create(MapToViewModel<T>(filteredUserSensorEntities), pageIndex, pageSize, minPollingInterval);
		}

		private IQueryable<UserSensorEntity> GetFilteredQuery(IQueryable<UserSensorEntity> userSensorEntitiesQuery, string measureTypeFilter = null, bool? isPublic = null, bool? isAlarmOn = null, string searchTerm = null)
		{
			if (!string.IsNullOrEmpty(searchTerm) || !string.IsNullOrWhiteSpace(searchTerm))
			{
				userSensorEntitiesQuery = userSensorEntitiesQuery
					.Where(x => x.Name.Contains(searchTerm) || x.Description.Contains(searchTerm));
			}

			if (!string.IsNullOrEmpty(measureTypeFilter) || !string.IsNullOrWhiteSpace(measureTypeFilter))
			{
				userSensorEntitiesQuery = userSensorEntitiesQuery
					.Where(x => x.Sensor.SensorProperty.MeasureType == measureTypeFilter);
			}

			if (isPublic.HasValue)
			{
				userSensorEntitiesQuery = userSensorEntitiesQuery.Where(x => x.IsPublic == isPublic.Value);
			}
			
			if (isAlarmOn.HasValue)
			{
				userSensorEntitiesQuery = userSensorEntitiesQuery.Where(x => x.IsAlarmOn == isAlarmOn.Value);
			}

			return userSensorEntitiesQuery;
		}

		public async Task<int> GetCountAsync(bool includePrivate = false)
		{
			return includePrivate
				? await _dbContext.UserSensors
                    .CountAsync()
				: await _dbContext.UserSensors
                    .CountAsync(x => x.IsPublic);
		}

		public async Task<T> GetAsync<T>(int id)
		{
			var userSensorEntity = await _dbContext.UserSensors
				.Include(x => x.Sensor)
				.ThenInclude(x => x.SensorProperty)
				.FirstOrDefaultAsync(x => x.Id == id);

			if (userSensorEntity == null)
			{
				throw new Exception($"userSensorEntity with Id: {id} was not found in the database!");
			}

			return MapToViewModel<T>(userSensorEntity);
		}

		public async Task UpdateAsync(CreateUpdateUserSensorViewModel userSensorViewModel)
		{
			var userSensorEntity = await this._dbContext.UserSensors
				.FirstOrDefaultAsync(x => x.Id == userSensorViewModel.Id);

			if (userSensorEntity == null)
			{
				throw new Exception($"UserSensorEntity with Id: {userSensorEntity.Id} was not found in the database!");
			}

			userSensorEntity.Description = userSensorViewModel.Description;
			if (!userSensorViewModel.IsSwitch)
			{
				userSensorEntity.IsAlarmOn = userSensorViewModel.IsAlarmOn;
				if (!userSensorViewModel.IsAlarmOn)
				{
					userSensorEntity.MinRangeValue = null;
					userSensorEntity.MaxRangeValue = null;
				}
				else
				{
					userSensorEntity.MinRangeValue = userSensorViewModel.CustomMinRangeValue;
					userSensorEntity.MaxRangeValue = userSensorViewModel.CustomMaxRangeValue;
				}
			}
			userSensorEntity.IsPublic = userSensorViewModel.IsPublic;
			userSensorEntity.Latitude = userSensorViewModel.Latitude;
			userSensorEntity.Longitude = userSensorViewModel.Longitude;
			userSensorEntity.Name = userSensorViewModel.Name;
			userSensorEntity.PollingInterval = userSensorViewModel.CustomPollingInterval;

			this._dbContext.UserSensors
				.Update(userSensorEntity);

			await this._dbContext.SaveChangesAsync();
		}

		public async Task<int> GetCountByUserIdAsync(int userId)
		{
			return await _dbContext.UserSensors
					.CountAsync(x => x.UserId == userId);
		}

		private IEnumerable<T> MapToViewModel<T>(IEnumerable<UserSensorEntity> userSensorViewModels)
		{
			List<T> userSensors = new List<T>();

			foreach (var userSensor in userSensorViewModels)
			{
				userSensors.Add(_mapper.Map<T>(userSensor));
			}

			return userSensors;
		}

		private T MapToViewModel<T>(UserSensorEntity userSensorEntity)
		{
			T userSensor = _mapper.Map<T>(userSensorEntity);

			return userSensor;
		}

		private UserSensorEntity MapToEntity<T>(T userSensorViewModel)
		{
			return _mapper.Map<UserSensorEntity>(userSensorViewModel);
		}
	}
}
