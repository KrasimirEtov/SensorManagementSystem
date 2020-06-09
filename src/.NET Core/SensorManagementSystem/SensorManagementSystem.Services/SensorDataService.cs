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
		private readonly SensorManagementSystemDbContext _dbContext;
		private readonly IMapper _mapper;
		private readonly Random _random;

		public SensorDataService(SensorManagementSystemDbContext dbContext, IMapper mapper)
		{
			this._dbContext = dbContext;
			this._mapper = mapper;
			this._random = new Random();
		}

		public async Task<IEnumerable<SensorDataDTO>> GetAllAsync()
		{
			var sensors = await this._dbContext.Sensors
				.Include(x => x.SensorProperty)
				.ToListAsync();

			var sensorDataDTOs = MapToDTO(sensors);

			return PopulateValue(sensorDataDTOs);
		}

		private IEnumerable<SensorDataDTO> PopulateValue(List<SensorDataDTO> sensorDataDTOs)
		{
			foreach (var sensorDataDTO in sensorDataDTOs)
			{
				sensorDataDTO.Value = GenerateValue(sensorDataDTO.IsSwitch, sensorDataDTO.MinRangeValue, sensorDataDTO.MaxRangeValue);
			}

			return sensorDataDTOs;
		}

		private string GenerateValue(bool isSwitch, double? minRange = null, double? maxRange = null)
		{
			return isSwitch
				? (_random.NextDouble() * (maxRange - minRange) + minRange).ToString()
				: (_random.Next() > (int.MaxValue / 2)).ToString();
		}

		private List<SensorDataDTO> MapToDTO(IEnumerable<SensorEntity> sensorEntities)
		{
			List<SensorDataDTO> sensorDataDTOs = new List<SensorDataDTO>();

			if (sensorEntities != null)
			{
				foreach (var sensorEntity in sensorEntities)
				{
					sensorDataDTOs.Add(_mapper.Map<SensorDataDTO>(sensorEntity));
				}
			}

			return sensorDataDTOs;
		}
	}
}
