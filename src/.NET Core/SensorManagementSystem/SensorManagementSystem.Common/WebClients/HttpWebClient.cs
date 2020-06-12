using System;
using System.Net.Http;
using System.Threading.Tasks;
using SensorManagementSystem.Common.Extensions;
using SensorManagementSystem.Common.WebClients.Contract;

namespace SensorManagementSystem.Common.WebClients
{
	public class HttpWebClient : IHttpWebClient
	{
		private readonly IHttpClientFactory _clientFactory;
		private HttpClient _httpClient;

		public HttpWebClient(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
		}

		public async Task<string> GetAsStringAsync(string clientName, string requestUri)
		{
			CreateClient(clientName);

			var response = await _httpClient.GetAsync(requestUri);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();
		}

		public async Task<T> GetAsync<T>(string clientName, string requestUri)
		{
			CreateClient(clientName);

			var response = await _httpClient.GetAsync(requestUri);
			response.EnsureSuccessStatusCode();

			using (var responseStream = await response.Content.ReadAsStreamAsync())
			{
				return responseStream.CreateFromJsonStream<T>();
			}
		}

		private void CreateClient(string clientName)
		{
			if (string.IsNullOrEmpty(clientName) || string.IsNullOrWhiteSpace(clientName))
			{
				throw new Exception("Client name is required!");
			}

			_httpClient = _clientFactory.CreateClient(clientName);
		}
	}
}
