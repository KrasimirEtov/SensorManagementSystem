using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SensorManagementSystem.Common;

namespace SensorManagementSystem.App.Hubs
{
	public class SensorStoreHub : Hub
	{
		public override async Task OnConnectedAsync()
		{
			var currentUser = Context.User;
			var connectionId = Context.ConnectionId;

			if (currentUser.Identity.IsAuthenticated)
			{
				await Groups.AddToGroupAsync(connectionId, Constants.AuthenticationUsersGroupName);
			}

			await base.OnConnectedAsync();
		}
	}
}
