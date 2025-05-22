using Server.Data.Models;
using Server.Data;

namespace Server.Repositories
{
	public interface ISensorReadingRepository : IRepository<SensorReading> { }

	public class SensorReadingRepository : Repository<SensorReading>, ISensorReadingRepository
	{
		private readonly AppDbContext _context;
		public SensorReadingRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
