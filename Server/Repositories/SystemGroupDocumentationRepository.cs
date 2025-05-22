using Server.Data.Models;
using Server.Data;

namespace Server.Repositories
{
	public interface ISystemGroupDocumentationRepository : IRepository<SystemGroupDocumentation> { }

	public class SystemGroupDocumentationRepository : Repository<SystemGroupDocumentation>, ISystemGroupDocumentationRepository
	{
		private readonly AppDbContext _context;
		public SystemGroupDocumentationRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
