using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SensorManagementSystem.Data;
using SensorManagementSystem.Models.DTOs;
using SensorManagementSystem.Models.Entities;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.Services
{
	public class SensorService : ISensorService
	{
		private readonly SensorManagementSystemDbContext _dbContext;
		private readonly IMapper _mapper;

		public SensorService(SensorManagementSystemDbContext dbContext, IMapper mapper)
		{
			this._dbContext = dbContext;
			this._mapper = mapper;
		}

		public async Task<IEnumerable<T>> GetAllAsync<T>()
		{
			var sensorEntities = await this._dbContext.Sensors
				.Include(x => x.SensorProperty)
				.ToListAsync();

			return MapToDTO<T>(sensorEntities);
		}

		public async Task<int> GetCountAsync()
		{
			return await _dbContext.Sensors
				.CountAsync();
		}

		public async Task<IEnumerable<T>> GetAllFilteredAsync<T>(string measureTypeFilter = null)
		{
			var sensorEntities = this._dbContext.Sensors
				.Include(x => x.SensorProperty)
				.AsQueryable();

			if (measureTypeFilter != null)
			{
				sensorEntities = sensorEntities
					.Where(x => x.SensorProperty.MeasureType == measureTypeFilter);
			}
			var filteredSensorEntities = await sensorEntities.ToListAsync();

			return MapToDTO<T>(filteredSensorEntities);
		}

		public async Task<T> GetByIdAsync<T>(int id)
		{
			var sensorEntity = await this._dbContext.Sensors
				.Include(x => x.SensorProperty)
				.FirstOrDefaultAsync(x => x.Id == id);

			return MapToDTO<T>(sensorEntity);
		}

		public async Task CreateAsync(SensorDTO sensorDTO)
		{
			var sensorEntity = MapToEntity(sensorDTO);

			var existingEntity = await _dbContext.Sensors
				.Include(x => x.SensorProperty)
				.FirstOrDefaultAsync(x => x.SensorPropertyId == sensorDTO.SensorPropertyId && x.MinRangeValue == sensorDTO.MinRangeValue && x.MaxRangeValue == sensorDTO.MaxRangeValue && x.PollingInterval == sensorDTO.PollingInterval);

			if (existingEntity != null)
			{
				throw new Exception($"Sensor with Measure Type: {existingEntity.SensorProperty.MeasureType}, Polling Interval: {existingEntity.PollingInterval}, Min Range: {existingEntity.MinRangeValue} and Max Range: {existingEntity.MaxRangeValue} already exists!");
			}

			await this._dbContext.Sensors
				.AddAsync(sensorEntity);

			await this._dbContext.SaveChangesAsync();
		}

		public async Task UpdateAsync(SensorDTO sensorDTO)
		{
			var allSensors = await _dbContext.Sensors
				.Include(x => x.UserSensors)
				.Include(x => x.SensorProperty)
				.ToListAsync();

			var existingSensor = allSensors
				.FirstOrDefault(x => x.SensorPropertyId == sensorDTO.SensorPropertyId && x.MinRangeValue == sensorDTO.MinRangeValue && x.MaxRangeValue == sensorDTO.MaxRangeValue && x.Id != sensorDTO.Id);

			if (existingSensor != null)
			{
				throw new Exception($"Sensor with Measure Type: {existingSensor.SensorProperty.MeasureType}, Polling Interval: {existingSensor.PollingInterval}, Min Range: {existingSensor.MinRangeValue} and Max Range: {existingSensor.MaxRangeValue} already exists!");
			}

			var sensorEntity = allSensors
				.FirstOrDefault(x => x.Id == sensorDTO.Id);

			if (sensorEntity == null)
			{
				throw new Exception($"SensorEntity with Id: {sensorDTO.Id} was not found in the database!");
			}

			var sensorProperty = await this._dbContext.SensorProperties
				.FirstOrDefaultAsync(x => x.Id == sensorDTO.SensorPropertyId);

			if (sensorProperty == null)
			{
				throw new Exception($"SensorPropertyEntity with Id: {sensorDTO.SensorPropertyId} was not found in the database!");
			}

			sensorEntity.Description = sensorDTO.Description;
			sensorEntity.MaxRangeValue = sensorDTO.MaxRangeValue;
			sensorEntity.MinRangeValue = sensorDTO.MinRangeValue;
			sensorEntity.PollingInterval = sensorDTO.PollingInterval;
			sensorEntity.SensorPropertyId = sensorDTO.SensorPropertyId;

			this._dbContext.Sensors
				.Update(sensorEntity);

			await this._dbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var sensorEntity = await this._dbContext.Sensors
				.FirstOrDefaultAsync(x => x.Id == id);

			if (sensorEntity == null)
			{
				throw new Exception($"SensorEntity with Id: {id} was not found in the database!");
			}

			if (sensorEntity.UserSensors.Count > 0)
			{
				throw new Exception($"SensorEntity with Id: {id} has assigned User Sensors!");
			}

			this._dbContext.Remove(sensorEntity);

			await this._dbContext.SaveChangesAsync();
		}

		private IEnumerable<T> MapToDTO<T>(IEnumerable<SensorEntity> sensorEntities)
		{
			List<T> sensorDTOs = new List<T>();

			foreach (var sensorEntity in sensorEntities)
			{
				sensorDTOs.Add(_mapper.Map<T>(sensorEntity));
			}

			return sensorDTOs;
		}

		private T MapToDTO<T>(SensorEntity sensorEntity)
		{
			T sensorDTO = _mapper.Map<T>(sensorEntity);

			return sensorDTO;
		}

		private SensorEntity MapToEntity<T>(T sensorDTO)
		{
			return _mapper.Map<SensorEntity>(sensorDTO);
		}
	}
}
