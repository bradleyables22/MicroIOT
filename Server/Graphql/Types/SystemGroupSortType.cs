using HotChocolate.Data.Sorting;
using Server.Data.Models;

namespace Server.Graphql.Types
{
	public class SystemGroupSortType : SortInputType<SystemGroup>
	{
		protected override void Configure(ISortInputTypeDescriptor<SystemGroup> descriptor)
		{
			descriptor.BindFieldsExplicitly();
			descriptor.Field(x => x.SystemGroupID);
			descriptor.Field(x => x.Name);
			descriptor.Field(x => x.Description);
			descriptor.Field(x => x.CreatedOn);
			descriptor.Field(x => x.DeactivatedOn);
		}
	}
}
