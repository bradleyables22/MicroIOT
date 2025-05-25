using Server.Data;
using Server.Data.Models;
using Server.Graphql.Types;

namespace Server.Graphql
{
	public class Query
	{
		[UsePaging(MaxPageSize = 1)]
		[UseProjection]
		[UseFiltering]
		[UseSorting]
		[GraphQLDescription("Hierarchical query of each system")]
		public async Task<IQueryable<SystemGroup>> Systems([Service] AppDbContext _context) 
		{
			return  _context.SystemGroups;
		}
		[UsePaging(MaxPageSize = 5)]
		[UseProjection]
		[UseFiltering]
		[UseSorting]
		[GraphQLDescription("All group types")]
		public async Task<IQueryable<GroupType>> GroupTypes([Service] AppDbContext _context)
		{
			return _context.GroupTypes;
		}
		[UsePaging(MaxPageSize = 10)]
		[UseProjection]
		[UseFiltering]
		[UseSorting]
		[GraphQLDescription("All device types")]
		public async Task<IQueryable<DeviceType>> DeviceTypes([Service] AppDbContext _context)
		{
			return _context.DeviceTypes;
		}
		[UsePaging(MaxPageSize = 10)]
		[UseProjection]
		[UseFiltering]
		[UseSorting]
		[GraphQLDescription("All sensor types")]
		public async Task<IQueryable<SensorType>> SensorTypes([Service] AppDbContext _context)
		{
			return _context.SensorTypes;
		}
		[UsePaging(MaxPageSize = 10)]
		[UseProjection]
		[UseFiltering]
		[UseSorting]
		[GraphQLDescription("All sensor categories")]
		public async Task<IQueryable<SensorCategory>> SensorCategories([Service] AppDbContext _context)
		{
			return _context.SensorCategories;
		}
		[UsePaging(MaxPageSize = 10)]
		[UseProjection]
		[UseFiltering]
		[UseSorting]
		[GraphQLDescription("All reading types")]
		public async Task<IQueryable<ReadingType>> ReadingTypes([Service] AppDbContext _context)
		{
			return _context.ReadingTypes;
		}
	}
}
