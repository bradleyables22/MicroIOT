using Server.Data;
using Server.Data.Models;

namespace Server.Repositories
{
	public interface IOtaOverrideRepository : IRepository<OtaOverrideRecord> { }
	public class OtaOverrideRepository : Repository<OtaOverrideRecord>, IOtaOverrideRepository
	{
		private readonly AppDbContext _context;
		public OtaOverrideRepository(AppDbContext? context) : base(context)
		{
			_context = context;
		}
	}
}
