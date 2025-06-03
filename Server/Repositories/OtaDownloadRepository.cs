using Server.Data;
using Server.Data.Models;

namespace Server.Repositories
{
	public interface IOtaDownloadRepository : IRepository<OtaDownloadRecord> { }

	public class OtaDownloadRepository : Repository<OtaDownloadRecord>, IOtaDownloadRepository
	{
		private readonly AppDbContext _context;
		public OtaDownloadRepository(AppDbContext? context) : base(context)
		{
			_context = context;
		}
	}
}
