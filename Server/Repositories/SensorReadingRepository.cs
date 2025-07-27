using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.Models;
using Server.Models;
using Server.Repositories.Models;

namespace Server.Repositories
{
	public interface ISensorReadingRepository : IRepository<SensorReading> 
	{
		Task<RepositoryResponse<PagedResponse<SensorReading>>> GetPaged(string sensorID, long? afterReadingID, int take, string typeID);
	}

	public class SensorReadingRepository : Repository<SensorReading>, ISensorReadingRepository
	{
		private readonly AppDbContext _context;
		public SensorReadingRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<RepositoryResponse<PagedResponse<SensorReading>>> GetPaged(string sensorID, long? afterReadingID, int take, string typeID)
		{
			try
			{
				var query = _context.SensorReadings
					.Where(x => x.SensorID == sensorID)
					.Where(x=>x.ReadingTypeID == typeID);

				if (afterReadingID.HasValue)
					query = query.Where(x => x.ReadingID < afterReadingID);

				var results = await query
					.OrderByDescending(x => x.ReadingID)
					.Take(take)
					.ToListAsync();

				var count = results?.Count ?? 0;

				return new RepositoryResponse<PagedResponse<SensorReading>> { Success = true, Data = new PagedResponse<SensorReading>
					{
						Count = count,
						Last = results?.LastOrDefault()?.ReadingID,
						Data = results,
						End = count < take
					}
				};
			}
			catch (Exception e)
			{
				return new RepositoryResponse<PagedResponse<SensorReading>> { Success = false, Data = new PagedResponse<SensorReading> { Count = 0, Data = null, End = true, Last = null }, Exception = e };
			}
		}
	}
}
