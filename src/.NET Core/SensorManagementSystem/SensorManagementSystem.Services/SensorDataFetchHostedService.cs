using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SensorManagementSystem.Data;
using SensorManagementSystem.Models.DTOs;
using SensorManagementSystem.Models.Entities;
using SensorManagementSystem.Models.Enums;

namespace SensorManagementSystem.Services
{
	public class SensorDataFetchHostedService : BackgroundService
	{
		private readonly ILogger<SensorDataFetchHostedService> logger;
		private readonly IServiceScopeFactory scopeFactory;
		private Stopwatch stopWatch;
		private static HttpClient httpClient = new HttpClient();

		public SensorDataFetchHostedService(ILogger<SensorDataFetchHostedService> logger, IServiceScopeFactory scopeFactory)
		{
			this.logger = logger;
			this.scopeFactory = scopeFactory;
			this.stopWatch = new Stopwatch();
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			this.logger.LogInformation("Sensor data fetch service is starting.");
			
			stopWatch.Start();

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					await Task.Delay(TimeSpan.FromSeconds(10));

					var sensorDataDTOs = await FetchSensorDataAsync();

					if (sensorDataDTOs.Count < 1)
					{
						continue;
					}

					var userSensorEntities = await SaveSensorDataAsync(sensorDataDTOs);

					if (userSensorEntities != null && userSensorEntities.Count > 0)
					{
						int minPollingInterval = userSensorEntities.Min(x => x.PollingInterval);

						if (stopWatch.IsRunning && stopWatch.Elapsed > TimeSpan.FromSeconds(minPollingInterval))
						{
							NotifyHub(userSensorEntities, sensorDataDTOs);
							stopWatch.Restart();
						}
					}
				}
				catch (Exception ex)
				{
					this.logger.LogError(ex, "Error occured in Sensor data fetch service");
				}
			}
		}

		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			this.logger.LogInformation("Sensor data fetch service is stopping.");

			await base.StopAsync(cancellationToken);
		}

		private async Task<List<SensorDataDTO>> FetchSensorDataAsync()
		{
			List<SensorDataDTO> sensorDataDTOs = new List<SensorDataDTO>();

			var response = await httpClient.GetAsync("https://localhost:5001/api/sensordata/all");

			if (response.StatusCode == HttpStatusCode.OK)
			{
				var sensorDataJson = await response.Content.ReadAsStringAsync();

				sensorDataDTOs = JsonConvert.DeserializeObject<List<SensorDataDTO>>(sensorDataJson);

				return sensorDataDTOs;
			}
			else if (response.StatusCode == HttpStatusCode.NotFound)
			{
				// Fire signalR notification that the API is down and latest database value is used (if such is present)
			}

			return sensorDataDTOs;
		}

		private async Task<List<UserSensorEntity>> SaveSensorDataAsync(List<SensorDataDTO> sensorDataDTOs)
		{
			using (var scope = this.scopeFactory.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<SensorManagementSystemDbContext>();

				var userSensorEntities = await dbContext.UserSensors
					.Include(x => x.Sensor)
					.ThenInclude(x => x.SensorProperty)
					.ToListAsync();

				if (userSensorEntities != null && userSensorEntities.Count > 0)
				{
					foreach (var userSensorEntity in userSensorEntities)
					{
						var matchingSensorDataDTO = sensorDataDTOs
							.FirstOrDefault(x => x.SensorType == userSensorEntity.Sensor.SensorProperty.Type
							&& x.PollingInterval == userSensorEntity.Sensor.PollingInterval);

						userSensorEntity.Value = matchingSensorDataDTO.Value;
						userSensorEntity.UpdatedOn = DateTime.UtcNow;
					}

					dbContext.UserSensors
						.UpdateRange(userSensorEntities);

					await dbContext.SaveChangesAsync();
				}

				return userSensorEntities;
			}
		}

		private void NotifyHub(List<UserSensorEntity> userSensorEntities, List<SensorDataDTO> sensorDataDTOs)
		{
			foreach (var userSensorEntity in userSensorEntities)
			{
				var matchingSensorDataDTO = sensorDataDTOs
						.FirstOrDefault(x => x.SensorType == userSensorEntity.Sensor.SensorProperty.Type && x.PollingInterval == userSensorEntity.Sensor.PollingInterval);

				if (userSensorEntity.IsAlarmOn.HasValue && userSensorEntity.IsAlarmOn.Value)
				{
					if (userSensorEntity.Sensor.SensorProperty.Type != SensorType.Switch)
					{
						double latestValue = double.Parse(matchingSensorDataDTO.Value);

						if (latestValue < userSensorEntity.MinRangeValue || latestValue > userSensorEntity.MaxRangeValue)
						{
							// Send notification with SignalR to the hub
							// NotificationService(Manager).SendAsync();
						}
					}
				}
			}
		}
	}
}
