using System.Collections.Generic;
using System.Threading.Tasks;
using SensorManagementSystem.Models.DTOs;

namespace SensorManagementSystem.Services.Contract
{
	public interface ISensorService
	{
		Task<IEnumerable<T>> GetAllAsync<T>();

		Task<int> GetCountAsync();

		Task<IEnumerable<T>> GetAllFilteredAsync<T>(string measureTypeFilter = null);

		Task<T> GetByIdAsync<T>(int id);

		Task CreateAsync(SensorDTO sensorDTO);

		Task DeleteAsync(int id);

		Task UpdateAsync(SensorDTO sensorDTO);
	}
}
