using System.Collections.Generic;
using System.Threading.Tasks;
using SensorManagementSystem.Models.DTOs;
using SensorManagementSystem.Models.Entities;
using SensorManagementSystem.Models.ViewModels;

namespace SensorManagementSystem.Services.Contract
{
	public interface ISensorService
	{
		Task<IEnumerable<T>> GetAllAsync<T>();

		Task<PaginatedList<T>> GetAllFilteredAsync<T>(int pageIndex, int pageSize, string measureTypeFilter = null);

		Task<T> GetByIdAsync<T>(int id);

		Task CreateAsync(SensorDTO sensorDTO);

		Task DeleteAsync(int id);

		Task UpdateAsync(SensorDTO sensorDTO);
	}
}
