using Server.Data;
using Server.Data.Models;
using Server.Graphql.Types;

namespace Server.Graphql
{
	public class Query
	{
		[UsePaging]
		[UseProjection]
		[UseFiltering]
		//[UseSorting(typeof(SystemGroupSortType))]
		[UseSorting]
		public async Task<IQueryable<SystemGroup>> Systems([Service] AppDbContext _context) 
		{
			return  _context.SystemGroups;
		}
	}
}
