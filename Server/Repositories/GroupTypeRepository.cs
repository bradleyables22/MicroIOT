using Server.Data.Models;
using Server.Data;

namespace Server.Repositories
{
	public interface IGroupTypeRepository : IRepository<DeviceGroupType> { }

	public class GroupTypeRepository : Repository<DeviceGroupType>, IGroupTypeRepository
	{
		private readonly AppDbContext _context;
		public GroupTypeRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
