﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SensorManagementSystem.Models.DTOs;

namespace SensorManagementSystem.Services.Contract
{
	public interface ISensorPropertyService
	{
		Task<IEnumerable<T>> GetAllAsync<T>();

		Task<T> GetByIdAsync<T>(int id);

		Task<int> GetCountAsync();

		Task<IEnumerable<string>> GetSensorMeasureTypesAsync();

		Task CreateAsync(SensorPropertyDTO sensorPropertyDTO);

		Task DeleteAsync(int id);

		Task UpdateAsync(SensorPropertyDTO sensorPropertyDTO);
	}
}
