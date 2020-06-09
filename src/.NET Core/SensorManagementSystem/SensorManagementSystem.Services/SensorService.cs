﻿using System;
using System.Collections.Generic;
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
		private readonly SensorManagementSystemDbContext dbContext;
		private readonly IMapper mapper;

		public SensorService(SensorManagementSystemDbContext dbContext, IMapper mapper)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
		}

		public async Task<IEnumerable<SensorDTO>> GetAllAsync()
		{
			var sensorEntities = await this.dbContext.Sensors
				.Include(x => x.SensorProperty)
				.ToListAsync();

			return MapToDTO(sensorEntities);
		}

		public async Task<SensorDTO> GetByIdAsync(int id)
		{
			var sensorEntity = await this.dbContext.Sensors
				.Include(x => x.SensorProperty)
				.FirstOrDefaultAsync(x => x.Id == id);

			return MapToDTO(sensorEntity);
		}

		public async Task CreateAsync(SensorDTO sensorDTO)
		{
			var sensorEntity = MapToEntity(sensorDTO);

			await this.dbContext.Sensors
				.AddAsync(sensorEntity);

			await this.dbContext.SaveChangesAsync();
		}

		public async Task UpdateAsync(SensorDTO sensorDTO)
		{
			var sensorEntity = await this.dbContext.Sensors
				.Include(x => x.UserSensors)
				.Include(x => x.SensorProperty)
				.FirstOrDefaultAsync(x => x.Id == sensorDTO.Id);

			if (sensorEntity == null)
			{
				throw new Exception($"SensorEntity with Id: {sensorDTO.Id} was not found in the database!");
			}

			var sensorProperty = await this.dbContext.SensorProperties
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

			this.dbContext.Sensors
				.Update(sensorEntity);

			await this.dbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var sensorEntity = await this.dbContext.Sensors
				.FirstOrDefaultAsync(x => x.Id == id);

			if (sensorEntity == null)
			{
				throw new Exception($"SensorEntity with Id: {id} was not found in the database!");
			}

			this.dbContext.Remove(sensorEntity);

			await this.dbContext.SaveChangesAsync();
		}

		private IEnumerable<SensorDTO> MapToDTO(IEnumerable<SensorEntity> sensorEntities)
		{
			List<SensorDTO> sensorDTOs = new List<SensorDTO>();

			foreach (var sensorEntity in sensorEntities)
			{
				sensorDTOs.Add(mapper.Map<SensorDTO>(sensorEntity));
			}

			return sensorDTOs;
		}

		private SensorDTO MapToDTO(SensorEntity sensorEntity)
		{
			SensorDTO sensorDTO = mapper.Map<SensorDTO>(sensorEntity);

			return sensorDTO;
		}

		private SensorEntity MapToEntity(SensorDTO sensorDTO)
		{
			return mapper.Map<SensorEntity>(sensorDTO);
		}
	}
}