using System.Threading.Tasks;
using AutoMapper;
using SensorManagementSystem.Data;
using SensorManagementSystem.Models.Entities;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.Services
{
	public class UserSensorService : IUserSensorService
	{
		private readonly SensorManagementSystemDbContext _dbContext;
		private readonly IMapper _mapper;

		public UserSensorService(SensorManagementSystemDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task CreateAsync<T>(T model)
		{
			var dbEntity = _mapper.Map<UserSensorEntity>(model);

			await _dbContext.UserSensors
				.AddAsync(dbEntity);

			await _dbContext.SaveChangesAsync();
		}
	}
}
