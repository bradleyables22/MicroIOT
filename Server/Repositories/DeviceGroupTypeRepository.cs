using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.Models;

namespace Server.Repositories
{
	public interface IDeviceGroupTypeRepository : IRepository<DeviceGroupType> { }
	public class DeviceGroupTypeRepository : Repository<DeviceGroupType>, IDeviceGroupTypeRepository
	{
		private readonly AppDbContext _context;
		public DeviceGroupTypeRepository(AppDbContext? context) : base(context)
		{
			_context = context;
		}
	}
}
