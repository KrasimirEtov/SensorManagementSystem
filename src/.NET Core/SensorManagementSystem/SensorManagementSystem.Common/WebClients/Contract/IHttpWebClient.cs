using System.Threading.Tasks;

namespace SensorManagementSystem.Common.WebClients.Contract
{
	public interface IHttpWebClient
	{
		Task<string> GetAsStringAsync(string clientName, string requestUri);

		Task<T> GetAsync<T>(string clientName, string requestUri);
	}
}
