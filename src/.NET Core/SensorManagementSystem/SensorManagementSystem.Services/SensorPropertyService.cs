using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SensorManagementSystem.Data;
using SensorManagementSystem.Models.DTOs;
using SensorManagementSystem.Models.Entities;
using SensorManagementSystem.Models.Enums;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.Services
{
	public class SensorPropertyService : ISensorPropertyService
	{
		private readonly SensorManagementSystemDbContext dbContext;
		private readonly IMapper mapper;

		public SensorPropertyService(SensorManagementSystemDbContext dbContext, IMapper mapper)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
		}

		public async Task<IEnumerable<SensorPropertyDTO>> GetAllAsync()
		{
			var sensorPropertyEntities = await this.dbContext.SensorProperties
				.ToListAsync();

			return MapToDTO(sensorPropertyEntities);
		}

		public async Task<SensorPropertyDTO> GetByIdAsync(int id)
		{
			var sensorPropertyEntity = await this.dbContext.SensorProperties
				.FirstOrDefaultAsync(x => x.Id == id);

			return MapToDTO(sensorPropertyEntity);
		}

		public async Task<IEnumerable<SensorType>> GetSensorTypesAsync(bool useFilter = false)
		{
			var sensorTypes = (IEnumerable<SensorType>)Enum.GetValues(typeof(SensorType));

			if (useFilter)
			{
				var usedSensorTypes = await this.dbContext.SensorProperties
				.Select(x => x.Type)
				.ToListAsync();

				sensorTypes = sensorTypes.Except(usedSensorTypes);
			}

			return sensorTypes;
		}

		public async Task CreateAsync(SensorPropertyDTO sensorPropertyDTO)
		{
			var sensorPropertyEntity = MapToEntity(sensorPropertyDTO);

			await this.dbContext.SensorProperties
				.AddAsync(sensorPropertyEntity);

			await this.dbContext.SaveChangesAsync();
		}

		public async Task UpdateAsync(SensorPropertyDTO sensorPropertyDTO)
		{
			var sensorPropertyEntity = await this.dbContext.SensorProperties
				.Include(x => x.Sensors)
				.FirstOrDefaultAsync(x => x.Id == sensorPropertyDTO.Id);

			if (sensorPropertyEntity == null)
			{
				throw new Exception($"SensorPropertyEntity with Id: {sensorPropertyDTO.Id} was not found in the database!");
			}

			sensorPropertyEntity.MeasureUnit = sensorPropertyDTO.MeasureUnit;
			sensorPropertyEntity.Type = sensorPropertyDTO.Type;

			this.dbContext.SensorProperties
				.Update(sensorPropertyEntity);

			await this.dbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var sensorPropertyEntity = await this.dbContext.SensorProperties
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

			this.dbContext.Remove(sensorPropertyEntity);

			await this.dbContext.SaveChangesAsync();
		}

		private IEnumerable<SensorPropertyDTO> MapToDTO(IEnumerable<SensorPropertyEntity> sensorPropertyEntities)
		{
			List<SensorPropertyDTO> sensorPropertyDTOs = new List<SensorPropertyDTO>();

			foreach (var sensorEntity in sensorPropertyEntities)
			{
				sensorPropertyDTOs.Add(mapper.Map<SensorPropertyDTO>(sensorEntity));
			}

			return sensorPropertyDTOs;
		}

		private SensorPropertyDTO MapToDTO(SensorPropertyEntity sensorPropertyEntity)
		{
			SensorPropertyDTO sensorPropertyDTO = mapper.Map<SensorPropertyDTO>(sensorPropertyEntity);

			return sensorPropertyDTO;
		}

		private SensorPropertyEntity MapToEntity(SensorPropertyDTO sensorPropertyDTO)
		{
			return mapper.Map<SensorPropertyEntity>(sensorPropertyDTO);
		}
	}
}
