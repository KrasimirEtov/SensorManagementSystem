using System.Collections.Generic;
using System.Threading.Tasks;
using SensorManagementSystem.Models.DTOs;

namespace SensorManagementSystem.Services.Contract
{
	public interface ISensorDataService
	{
		Task<IEnumerable<SensorDataDTO>> GetAllAsync();
	}
}
