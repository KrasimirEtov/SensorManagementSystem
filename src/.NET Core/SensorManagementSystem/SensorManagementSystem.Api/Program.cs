using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace SensorManagementSystem.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
			try
			{
				logger.Debug("Starting Api.");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Stopped because of exception.");
			}
			finally
			{
				NLog.LogManager.Shutdown();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
			.ConfigureLogging(builder =>
			{
				builder.ClearProviders();
				builder.SetMinimumLevel(LogLevel.Trace);
				builder.AddConsole();
			})
			.UseNLog();
	}
}
