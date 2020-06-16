using System.Threading.Tasks;
using AutoMapper;
using SensorManagementSystem.Data;
using SensorManagementSystem.Models.ViewModels;
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
	}
}
