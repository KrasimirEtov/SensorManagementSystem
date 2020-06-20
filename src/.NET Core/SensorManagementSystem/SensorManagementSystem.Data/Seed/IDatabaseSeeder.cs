using System.Threading.Tasks;

namespace SensorManagementSystem.Data.Seed
{
	public interface IDatabaseSeeder
	{
		Task MigrateDatabaseAsync();

		Task SeedAdminAsync();

		Task SeedRolesAsync();

		Task SeedSensorPropertiesAsync();
	}
}
