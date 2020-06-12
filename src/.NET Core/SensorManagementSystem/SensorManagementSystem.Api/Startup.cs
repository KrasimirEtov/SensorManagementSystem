using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SensorManagementSystem.Common.Extensions;
using SensorManagementSystem.Data;
using SensorManagementSystem.Models.AutoMapper;
using SensorManagementSystem.Services;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers()
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ContractResolver = new DefaultContractResolver
					{
						NamingStrategy = new CamelCaseNamingStrategy()
					};
					options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
					options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
					options.SerializerSettings.Converters.Add(new StringEnumConverter());
					options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
				});

			services.AddDbContext<SensorManagementSystemDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("SensorManagementSystem"));
			}, contextLifetime: ServiceLifetime.Transient);

			services.AddOptions();

			services.AddAutoMapper(typeof(MappingProfile));

			services.AddTransient<ISensorService, SensorService>();
			services.AddTransient<ISensorPropertyService, SensorPropertyService>();
			services.AddTransient<ISensorDataService, SensorDataService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}


			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseErrorLogging();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
