using System.Threading.Tasks;

namespace SensorManagementSystem.Services.Contract
{
	public interface IEmailService
	{
		Task SendAsync(string receiver, string subject, string body);
	}
}
