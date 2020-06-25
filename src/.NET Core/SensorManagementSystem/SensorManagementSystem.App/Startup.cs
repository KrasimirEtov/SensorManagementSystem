using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SensorManagementSystem.Data;
using SensorManagementSystem.Models.Entities;
using SensorManagementSystem.Services;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using SensorManagementSystem.Models.AutoMapper;
using AutoMapper;
using SensorManagementSystem.Common;
using SensorManagementSystem.Data.Seed;
using System;
using SensorManagementSystem.Common.WebClients.Contract;
using SensorManagementSystem.Common.WebClients;
using SensorManagementSystem.Services.Contract;
using SensorManagementSystem.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;

namespace SensorManagementSystem.App
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
			services.AddDbContext<SensorManagementSystemDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("SensorManagementSystem"));
			}, contextLifetime: ServiceLifetime.Transient);

			services.AddIdentity<UserEntity, RoleEntity>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequiredLength = 3;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
			})
				.AddEntityFrameworkStores<SensorManagementSystemDbContext>()
				.AddDefaultTokenProviders();

			services.AddAuthorizationCore(options =>
			{
				options.AddPolicy(Constants.AdminPolicyName, policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.RequireRole(Constants.AdminRoleName);
				});
			});

			services.AddControllersWithViews(options =>
			{
				//options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
			})
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
				})
				.AddRazorRuntimeCompilation();

			services.AddOptions();
			services.AddAutoMapper(typeof(MappingProfile));
			services.AddRazorPages()
				.AddRazorRuntimeCompilation();
			services.AddSignalR();

			services.AddHttpClient(Constants.SensorApiClientName, options =>
			{
				options.BaseAddress = new Uri("https://localhost:5001");
			});

			services.AddHostedService<SensorDataFetchHostedService>();
			services.AddTransient<ISensorDataService, SensorDataService>();
			services.AddTransient<IDatabaseSeeder, DatabaseSeeder>();
			services.AddTransient<IHttpWebClient, HttpWebClient>();
			services.AddTransient<ISensorService, SensorService>();
			services.AddTransient<ISensorPropertyService, SensorPropertyService>();
			services.AddTransient<IUserSensorService, UserSensorService>();
			services.AddSingleton<ICachingService, MemoryCacheService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDatabaseSeeder databaseSeeder)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseErrorLogging();

			app.UseAuthentication();
			app.UseAuthorization();

			databaseSeeder.MigrateDatabaseAsync().Wait(TimeSpan.FromSeconds(10));
			databaseSeeder.SeedAdminAsync().Wait(TimeSpan.FromSeconds(10));
			databaseSeeder.SeedRolesAsync().Wait(TimeSpan.FromSeconds(10));
			databaseSeeder.SeedSensorPropertiesAsync().Wait(TimeSpan.FromSeconds(10));

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapAreaControllerRoute(
					name: "My" + Constants.AdminAreaName,
					areaName: Constants.AdminAreaName,
					pattern: Constants.AdminAreaName + "/{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
		}
	}
}
