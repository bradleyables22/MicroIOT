using Server.Data.Models;
using Server.Data;

namespace Server.Repositories
{
	public interface IDeviceSensorRepository : IRepository<DeviceSensor> { }

	public class DeviceSensorRepository : Repository<DeviceSensor>, IDeviceSensorRepository
	{
		private readonly AppDbContext _context;
		public DeviceSensorRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
