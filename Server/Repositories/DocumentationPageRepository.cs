using Server.Data.Models;
using Server.Data;

namespace Server.Repositories
{
	public interface IDocumentationPageRepository : IRepository<DocumentationPage> { }

	public class DocumentationPageRepository : Repository<DocumentationPage>, IDocumentationPageRepository
	{
		private readonly AppDbContext _context;
		public DocumentationPageRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}

}
