using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Data.Models;
using System.Text.Json;

namespace Server.Extensions
{
	public static class JsonExtensions
	{
		
	}
	public static class JsonValueConverter
	{
		private static readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.General);

		public static ValueConverter<List<Entry>?, string> ListOfEntryConverter =>
			new(
				v => JsonSerializer.Serialize(v ?? new(), _options),
				v => JsonSerializer.Deserialize<List<Entry>?>(v, _options) ?? new()
			);
	}
}
