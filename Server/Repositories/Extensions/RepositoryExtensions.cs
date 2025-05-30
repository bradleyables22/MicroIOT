using Server.Data;
using Server.Repositories.Models;
using System.Data;
using System.Reflection;

namespace Server.Repositories.Extensions
{
	public static class RepositoryExtensions
	{
		public static void AddRepositories(this IServiceCollection services) 
		{
			services.AddScoped<ISystemGroupRepository, SystemGroupRepository>();
			services.AddScoped<IDeviceGroupRepository, DeviceGroupRepository>();
			services.AddScoped<IDeviceRepository, DeviceRepository>();
			services.AddScoped<IDeviceSensorRepository, DeviceSensorRepository>();
			services.AddScoped<IDeviceTypeRepository, DeviceTypeRepository>();
			services.AddScoped<IDeviceGroupTypeRepository, DeviceGroupTypeRepository>();
			services.AddScoped<ISensorCategoryRepository, SensorCategoryRepository>();
			services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();
			services.AddScoped<ISensorTypeRepository, SensorTypeRepository>();
			services.AddScoped<IReadingTypeRepository, ReadingTypeRepository>();
		}

		public static async Task<RepositoryResponse<T>> GetResponseAsync<T>(this Task<T> task)
		{
			var response = new RepositoryResponse<T>();
			try
			{
				var data = await task;
				response.Data = data;
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Data = default;
				response.Exception = ex;
			}

			return response;
		}

		public static async Task<RepositoryResponse<T>> GetResponseAsync<T>(this ValueTask<T> valueTask)
			=> await valueTask.AsTask().GetResponseAsync();

		public static void CopyProperties<T>(this T source, T target, bool ignoreNulls = true)
		{
			if (source == null || target == null)
				throw new ArgumentNullException("Source or/and Target object(s) are null");

			Type type = typeof(T);
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (PropertyInfo property in properties)
			{
				if (property.CanRead && property.CanWrite)
				{
					var value = property.GetValue(source);
					if (ignoreNulls)
					{
						if (value != null)
							property.SetValue(target, value);
					}
					else
						property.SetValue(target, value);
				}
			}
		}

		public static async Task<RepositoryResponse<T>> GetSavedChangesAsync<T>(this AppDbContext context, T entity,bool isDeleteMethod = false)
		{
			var response = new RepositoryResponse<T>();
			try
			{
				var result = await context.SaveChangesAsync();
				if (result == 0 && !isDeleteMethod)
					throw new DataException("Unable to create, update, or remove entity.");
				if (isDeleteMethod)
					context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
				response.Data = entity;
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Data = default;
				response.Exception = ex;
			}

			return response;
		}
	}
}
