using Server.Data.Models;
using Server.Data;

namespace Server.Repositories
{
	public interface ISensorTypeRepository : IRepository<SensorType> { }

	public class SensorTypeRepository : Repository<SensorType>, ISensorTypeRepository
	{
		private readonly AppDbContext _context;
		public SensorTypeRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
