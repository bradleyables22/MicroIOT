using Server.Data;
using Server.Data.Models;

namespace Server.Repositories
{
	public interface ISystemGroupRepository:IRepository<SystemGroup> { }
	public class SystemGroupRepository : Repository<SystemGroup>, ISystemGroupRepository
	{
		private readonly AppDbContext _context;
		public SystemGroupRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
