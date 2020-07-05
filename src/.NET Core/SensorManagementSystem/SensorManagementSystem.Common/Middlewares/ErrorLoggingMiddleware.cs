using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SensorManagementSystem.Common.Middlewares
{
	public class ErrorLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;

		public ErrorLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
		{
			this._next = next;
			this._logger = loggerFactory.CreateLogger<ErrorLoggingMiddleware>();
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await this._next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Exception was thrown.");
				throw;
			}
		}
	}
}
