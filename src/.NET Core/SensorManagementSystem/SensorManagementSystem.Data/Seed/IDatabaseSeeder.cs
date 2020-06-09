using System.Threading.Tasks;

namespace SensorManagementSystem.Data.Seed
{
	public interface IDatabaseSeeder
	{
		Task SeedAdmin();

		Task SeedRoles();

		Task SeedSensorProperies();
	}
}
