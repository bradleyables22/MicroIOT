using Server.Data.Models;
using Server.Data;

namespace Server.Repositories
{
	public interface IDeviceGroupRepository : IRepository<DeviceGroup> { }

	public class DeviceGroupRepository : Repository<DeviceGroup>, IDeviceGroupRepository
	{
		private readonly AppDbContext _context;
		public DeviceGroupRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
