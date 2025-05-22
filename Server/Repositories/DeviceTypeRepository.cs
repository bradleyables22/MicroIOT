using Server.Data.Models;
using Server.Data;

namespace Server.Repositories
{
	public interface IDeviceTypeRepository : IRepository<DeviceType> { }

	public class DeviceTypeRepository : Repository<DeviceType>, IDeviceTypeRepository
	{
		private readonly AppDbContext _context;
		public DeviceTypeRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
