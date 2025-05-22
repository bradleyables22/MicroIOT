using Server.Data.Models;
using Server.Data;

namespace Server.Repositories
{
	public interface ISensorCategoryRepository : IRepository<SensorCategory> { }

	public class SensorCategoryRepository : Repository<SensorCategory>, ISensorCategoryRepository
	{
		private readonly AppDbContext _context;
		public SensorCategoryRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
