using Server.Data.Models;
using Server.Data;

namespace Server.Repositories
{
	public interface IReadingTypeRepository : IRepository<ReadingType> { }

	public class ReadingTypeRepository : Repository<ReadingType>, IReadingTypeRepository
	{
		private readonly AppDbContext _context;
		public ReadingTypeRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
