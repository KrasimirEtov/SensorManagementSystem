using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SensorManagementSystem.Common.Middlewares
{
	public class ErrorLoggingMiddleware
	{
		private readonly RequestDelegate next;
		private readonly ILogger logger;

		public ErrorLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
		{
			this.next = next;
			this.logger = loggerFactory.CreateLogger<ErrorLoggingMiddleware>();
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await this.next(context);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Exception was thrown.");
				throw;
			}
		}
	}
}
