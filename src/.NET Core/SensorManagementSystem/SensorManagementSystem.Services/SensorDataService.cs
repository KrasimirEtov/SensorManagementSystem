using System;
using System.Collections.Generic;
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
	public class SensorDataService : ISensorDataService
	{
		private readonly SensorManagementSystemDbContext dbContext;
		private readonly IMapper mapper;
		private readonly Random random;

		public SensorDataService(SensorManagementSystemDbContext dbContext, IMapper mapper)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
			this.random = new Random();
		}

		public async Task<IEnumerable<SensorDataDTO>> GetAllAsync()
		{
			var sensors = await this.dbContext.Sensors
				.Include(x => x.SensorProperty)
				.ToListAsync();

			var sensorDataDTOs = MapToDTO(sensors);

			return PopulateValue(sensorDataDTOs);
		}

		private IEnumerable<SensorDataDTO> PopulateValue(List<SensorDataDTO> sensorDataDTOs)
		{
			foreach (var sensorDataDTO in sensorDataDTOs)
			{
				sensorDataDTO.Value = GenerateValue(sensorDataDTO.SensorType, sensorDataDTO.MinRangeValue, sensorDataDTO.MaxRangeValue);
			}

			return sensorDataDTOs;
		}

		public string GenerateValue(SensorType sensorType, double? minRange = null, double? maxRange = null)
		{
			return sensorType != SensorType.Switch
				? (random.NextDouble() * (maxRange - minRange) + minRange).ToString()
				: (random.Next() > (int.MaxValue / 2)).ToString();
		}

		private List<SensorDataDTO> MapToDTO(IEnumerable<SensorEntity> sensorEntities)
		{
			List<SensorDataDTO> sensorDataDTOs = new List<SensorDataDTO>();

			if (sensorEntities != null)
			{
				foreach (var sensorEntity in sensorEntities)
				{
					sensorDataDTOs.Add(mapper.Map<SensorDataDTO>(sensorEntity));
				}
			}

			return sensorDataDTOs;
		}
	}
}
