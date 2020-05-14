using System.Collections.Generic;
using System.Threading.Tasks;
using SensorManagementSystem.Models.DTOs;

namespace SensorManagementSystem.Services.Contract
{
	public interface ISensorService
	{
		Task<IEnumerable<SensorDTO>> GetAllAsync();

		Task<SensorDTO> GetByIdAsync(int id);

		Task CreateAsync(SensorDTO sensorDTO);

		Task DeleteAsync(int id);

		Task UpdateAsync(SensorDTO sensorDTO);
	}
}
