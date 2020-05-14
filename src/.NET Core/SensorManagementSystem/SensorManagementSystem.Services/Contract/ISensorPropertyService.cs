using System.Collections.Generic;
using System.Threading.Tasks;
using SensorManagementSystem.Models.DTOs;
using SensorManagementSystem.Models.Enums;

namespace SensorManagementSystem.Services.Contract
{
	public interface ISensorPropertyService
	{
		Task<IEnumerable<SensorPropertyDTO>> GetAllAsync();

		Task<SensorPropertyDTO> GetByIdAsync(int id);

		Task<IEnumerable<SensorType>> GetSensorTypesAsync(bool useFilter = false);

		Task CreateAsync(SensorPropertyDTO sensorPropertyDTO);

		Task DeleteAsync(int id);

		Task UpdateAsync(SensorPropertyDTO sensorPropertyDTO);
	}
}
