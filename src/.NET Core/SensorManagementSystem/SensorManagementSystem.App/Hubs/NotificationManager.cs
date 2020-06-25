using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SensorManagementSystem.App.Hubs.Contract;
using SensorManagementSystem.Common;

namespace SensorManagementSystem.App.Hubs
{
	public class NotificationManager : INotificationManager
	{
		private readonly IHubContext<SensorStoreHub> _hub;

		public NotificationManager(IHubContext<SensorStoreHub> hub)
		{
			_hub = hub;
		}

		public async Task SendToAuthenticatedUsersAsync(string message)
		{
			await _hub.Clients
				.Groups(Constants.AuthenticationUsersGroupName)
				.SendAsync("ReceiveAuthenticatedUsersMessage", message);
		}
	}
}
