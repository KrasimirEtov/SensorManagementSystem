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
	public class SensorPropertyService : ISensorPropertyService
	{
		private readonly SensorManagementSystemDbContext _dbContext;
		private readonly IMapper _mapper;

		public SensorPropertyService(SensorManagementSystemDbContext dbContext, IMapper mapper)
		{
			this._dbContext = dbContext;
			this._mapper = mapper;
		}

		public async Task<IEnumerable<T>> GetAllAsync<T>()
		{
			var sensorPropertyEntities = await this._dbContext.SensorProperties
				.ToListAsync();

			return MapToDTO<T>(sensorPropertyEntities);
		}

		public async Task<T> GetByIdAsync<T>(int id)
		{
			var sensorPropertyEntity = await this._dbContext.SensorProperties
				.FirstOrDefaultAsync(x => x.Id == id);

			return MapToDTO<T>(sensorPropertyEntity);
		}

		public async Task<IEnumerable<string>> GetSensorMeasureTypesAsync()
		{
			var sensorTypes = await this._dbContext.SensorProperties
			.Select(x => x.MeasureType)
			.ToListAsync();

			return sensorTypes;
		}

		public async Task CreateAsync(SensorPropertyDTO sensorPropertyDTO)
		{
			var sensorPropertyEntity = MapToEntity(sensorPropertyDTO);

			await this._dbContext.SensorProperties
				.AddAsync(sensorPropertyEntity);

			await this._dbContext.SaveChangesAsync();
		}

		public async Task UpdateAsync(SensorPropertyDTO sensorPropertyDTO)
		{
			var sensorPropertyEntity = await this._dbContext.SensorProperties
				.Include(x => x.Sensors)
				.FirstOrDefaultAsync(x => x.Id == sensorPropertyDTO.Id);

			if (sensorPropertyEntity == null)
			{
				throw new Exception($"SensorPropertyEntity with Id: {sensorPropertyDTO.Id} was not found in the database!");
			}

			sensorPropertyEntity.MeasureUnit = sensorPropertyDTO.MeasureUnit;
			sensorPropertyEntity.MeasureType = sensorPropertyDTO.MeasureType;
			sensorPropertyEntity.IsSwitch = sensorPropertyDTO.IsSwitch;

			this._dbContext.SensorProperties
				.Update(sensorPropertyEntity);

			await this._dbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var sensorPropertyEntity = await this._dbContext.SensorProperties
				.Include(x => x.Sensors)
				.FirstOrDefaultAsync(x => x.Id == id);

			if (sensorPropertyEntity == null)
			{
				throw new Exception($"SensorPropertyEntity with Id: {id} was not found in the database!");
			}

			if (sensorPropertyEntity.Sensors != null && sensorPropertyEntity.Sensors.Count > 0)
			{
				throw new Exception($"Cannot delete SensorPropertyEntity with Id: {id} because it has created sensors. Consider deleting them first!");
			}

			this._dbContext.Remove(sensorPropertyEntity);

			await this._dbContext.SaveChangesAsync();
		}

		private IEnumerable<T> MapToDTO<T>(IEnumerable<SensorPropertyEntity> sensorPropertyEntities)
		{
			List<T> sensorPropertyDTOs = new List<T>();

			foreach (var sensorPropertyEntity in sensorPropertyEntities)
			{
				sensorPropertyDTOs.Add(_mapper.Map<T>(sensorPropertyEntity));
			}

			return sensorPropertyDTOs;
		}

		private T MapToDTO<T>(SensorPropertyEntity sensorPropertyEntity)
		{
			T sensorPropertyDTO = _mapper.Map<T>(sensorPropertyEntity);

			return sensorPropertyDTO;
		}

		private SensorPropertyEntity MapToEntity<T>(T sensorPropertyDTO)
		{
			return _mapper.Map<SensorPropertyEntity>(sensorPropertyDTO);
		}
	}
}
