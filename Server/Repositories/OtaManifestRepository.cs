using Server.Data;
using Server.Data.Models;

namespace Server.Repositories
{
	public interface IOtaManifestRepository : IRepository<OtaManifestRecord> { }
	public class OtaManifestRepository : Repository<OtaManifestRecord>, IOtaManifestRepository
	{
		private readonly AppDbContext _context;
		public OtaManifestRepository(AppDbContext? context) : base(context)
		{
			_context = context;
		}
	}
}
