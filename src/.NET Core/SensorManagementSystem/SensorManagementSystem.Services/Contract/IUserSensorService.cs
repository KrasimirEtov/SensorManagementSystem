using System.Collections.Generic;
using System.Threading.Tasks;
using SensorManagementSystem.Models.ViewModels;

namespace SensorManagementSystem.Services.Contract
{
	public interface IUserSensorService
	{
		Task CreateAsync<T>(T model);

		Task<T> GetAsync<T>(int id);

		Task<int> GetCountAsync(bool includePrivate = false);

		Task UpdateAsync(CreateUpdateUserSensorViewModel userSensorViewModel);

		Task<IEnumerable<T>> GetAllByUserIdAsync<T>(int userId);

		Task<int> GetCountByUserIdAsync(int userId);

		Task<PaginatedList<T>> GetAllFilteredAsync<T>(int userId, int pageIndex, int pageSize, string measureType = null, bool? isPublic = null, bool? isAlarmOn = null, string searchTerm = null);
	}
}
