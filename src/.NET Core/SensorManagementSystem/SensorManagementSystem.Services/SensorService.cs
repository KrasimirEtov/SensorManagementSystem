using System;
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
		private readonly SensorManagementSystemDbContext _dbContext;
		private readonly IMapper _mapper;

		public SensorService(SensorManagementSystemDbContext dbContext, IMapper mapper)
		{
			this._dbContext = dbContext;
			this._mapper = mapper;
		}

		public async Task<IEnumerable<SensorDTO>> GetAllAsync()
		{
			var sensorEntities = await this._dbContext.Sensors
				.Include(x => x.SensorProperty)
				.ToListAsync();

			return MapToDTO(sensorEntities);
		}

		public async Task<SensorDTO> GetByIdAsync(int id)
		{
			var sensorEntity = await this._dbContext.Sensors
				.Include(x => x.SensorProperty)
				.FirstOrDefaultAsync(x => x.Id == id);

			return MapToDTO(sensorEntity);
		}

		public async Task CreateAsync(SensorDTO sensorDTO)
		{
			var sensorEntity = MapToEntity(sensorDTO);

			await this._dbContext.Sensors
				.AddAsync(sensorEntity);

			await this._dbContext.SaveChangesAsync();
		}

		public async Task UpdateAsync(SensorDTO sensorDTO)
		{
			var sensorEntity = await this._dbContext.Sensors
				.Include(x => x.UserSensors)
				.Include(x => x.SensorProperty)
				.FirstOrDefaultAsync(x => x.Id == sensorDTO.Id);

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

			this._dbContext.Remove(sensorEntity);

			await this._dbContext.SaveChangesAsync();
		}

		private IEnumerable<SensorDTO> MapToDTO(IEnumerable<SensorEntity> sensorEntities)
		{
			List<SensorDTO> sensorDTOs = new List<SensorDTO>();

			foreach (var sensorEntity in sensorEntities)
			{
				sensorDTOs.Add(_mapper.Map<SensorDTO>(sensorEntity));
			}

			return sensorDTOs;
		}

		private SensorDTO MapToDTO(SensorEntity sensorEntity)
		{
			SensorDTO sensorDTO = _mapper.Map<SensorDTO>(sensorEntity);

			return sensorDTO;
		}

		private SensorEntity MapToEntity(SensorDTO sensorDTO)
		{
			return _mapper.Map<SensorEntity>(sensorDTO);
		}
	}
}
