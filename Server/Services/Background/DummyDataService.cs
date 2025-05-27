
using Server.Data;
using Server.Data.Models;

namespace Server.Services.Background
{
	public class DummyDataService : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;

		public DummyDataService(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			try
			{

				using var scope = _scopeFactory.CreateScope();
				var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
				var dummySystem = new SystemGroup
				{
					SystemGroupID = 1,
					Name = "Dummy Corp.",
					Description = "This is a dummy telematics system.",
					Metadata = new List<Entry> 
					{
						new Entry{ Key = "Company",Value="Telematics Corp."},
						new Entry{ Key = "Street 1",Value="555 Teli Way"},
						new Entry{ Key = "City",Value="Ferdinand"},
						new Entry{ Key = "State",Value="IN"},
						new Entry{ Key = "Zip",Value="47532"}
					}
				};
				_context.SystemGroups.Add(dummySystem);

				List<DeviceGroupType> dummygroupTypes = new ()
				{
					new DeviceGroupType
					{
						GroupTypeID = 1,
						Name = "Dry Van Trailer",
						Description = "Standard Trailer Type",
						Metadata = new List<Entry>
							{
								new Entry{ Key = "Area",Value="Trailers"},
								new Entry{ Key = "IsAreaDefault",Value="true"}
							}
					},
					new DeviceGroupType
					{
						GroupTypeID = 2,
						Name = "Sailboat",
						Description = "A primitive boat propelled by sails",
						Metadata = new List<Entry>
							{
								new Entry{ Key = "Area",Value="Watercraft"},
								new Entry{ Key = "IsAreaDefault",Value="true"}
							}
					},
				};

				foreach (var groupType in dummygroupTypes) 
				{
					_context.DeviceGroupTypes.Add(groupType);
				}

				List<DeviceGroup> dummyDeviceGroups = new() 
				{
					new DeviceGroup
					{
						GroupTypeID = 2,
						Alias = "Sail Boat 1",
						GroupID = "A1D4K41",
						Metadata = new List<Entry>
							{
								new Entry{ Key = "IsAreaDefault",Value="true" }
							}
					},
					new DeviceGroup
					{
						GroupTypeID = 1,
						Alias = "Trailer 1",
						GroupID = "463A44-VF",
						Metadata = new List<Entry>
							{
								new Entry{ Key = "VIN",Value="AAAAAA111222DDDEE"}
							}
					}
				};

				foreach (var deviceGroup in dummyDeviceGroups)
				{
					_context.DeviceGroups.Add(deviceGroup);
				}




				await _context.SaveChangesAsync();

			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw;
			}
		}
	}
}
