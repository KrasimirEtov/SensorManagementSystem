using System.Collections.Generic;
using System.Threading.Tasks;
using SensorManagementSystem.Models.ViewModels;

namespace SensorManagementSystem.Services.Contract
{
	public interface IUserSensorService
	{
		Task CreateAsync<T>(T model);

		Task<T> GetAsync<T>(int id);

		Task UpdateAsync(CreateUpdateUserSensorViewModel userSensorViewModel);

		Task<IEnumerable<T>> GetAllByUserId<T>(int userId);
	}
}
