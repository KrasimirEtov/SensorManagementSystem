using System.Threading.Tasks;

namespace SensorManagementSystem.Services.Contract
{
	public interface IUserSensorService
	{
		Task CreateAsync<T>(T model);
	}
}
