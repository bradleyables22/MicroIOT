using Server.Data.Models;
using Server.Data;

namespace Server.Repositories
{
	public interface IGroupTypeRepository : IRepository<GroupType> { }

	public class GroupTypeRepository : Repository<GroupType>, IGroupTypeRepository
	{
		private readonly AppDbContext _context;
		public GroupTypeRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
