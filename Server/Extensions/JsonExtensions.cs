using Microsoft.EntityFrameworkCore.ChangeTracking;
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

		public static ValueComparer<List<Entry>?> ListOfEntryComparer =>
			new(
				(c1, c2) => JsonSerializer.Serialize(c1 ?? new(), _options) == JsonSerializer.Serialize(c2 ?? new(), _options),
				c => JsonSerializer.Serialize(c ?? new(), _options).GetHashCode(),
				c => JsonSerializer.Deserialize<List<Entry>>(JsonSerializer.Serialize(c ?? new(), _options), _options) ?? new()
			);
	}
}
