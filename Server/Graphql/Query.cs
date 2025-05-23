using Server.Data;
using Server.Data.Models;
using Server.Graphql.Types;

namespace Server.Graphql
{
	public class Query
	{
		[UseFiltering]
		[UseSorting(typeof(SystemGroupSortType))]
		public async Task<IQueryable<SystemGroup>> Systems([Service] AppDbContext _context) 
		{
			return  _context.SystemGroups;
		}
	}
}
