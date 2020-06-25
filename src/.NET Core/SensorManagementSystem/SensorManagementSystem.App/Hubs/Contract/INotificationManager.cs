using System.Threading.Tasks;

namespace SensorManagementSystem.App.Hubs.Contract
{
	public interface INotificationManager
	{
		Task SendToAuthenticatedUsersAsync(string message);
	}
}
