using Microsoft.AspNetCore.Builder;
using SensorManagementSystem.Common.Middlewares;

namespace SensorManagementSystem.Common.Extensions
{
	public static class IApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ErrorLoggingMiddleware>();
		}

		public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ErrorHandlerMiddleware>();
		}
	}
}
