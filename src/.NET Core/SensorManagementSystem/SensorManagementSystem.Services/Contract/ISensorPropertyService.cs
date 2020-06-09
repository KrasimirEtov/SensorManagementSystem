using System.Collections.Generic;
using System.Threading.Tasks;
using SensorManagementSystem.Models.DTOs;

namespace SensorManagementSystem.Services.Contract
{
	public interface ISensorPropertyService
	{
		Task<IEnumerable<SensorPropertyDTO>> GetAllAsync();

		Task<SensorPropertyDTO> GetByIdAsync(int id);

		Task<IEnumerable<string>> GetSensorMeasureTypesAsync();

		Task CreateAsync(SensorPropertyDTO sensorPropertyDTO);

		Task DeleteAsync(int id);

		Task UpdateAsync(SensorPropertyDTO sensorPropertyDTO);
	}
}
