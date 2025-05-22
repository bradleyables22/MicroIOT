using Server.Data.Models;
using Server.Data;

namespace Server.Repositories
{
	public interface IDeviceRepository : IRepository<Device> { }

	public class DeviceRepository : Repository<Device>, IDeviceRepository
	{
		private readonly AppDbContext _context;
		public DeviceRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
