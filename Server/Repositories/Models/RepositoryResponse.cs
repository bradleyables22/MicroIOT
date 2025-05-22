namespace Server.Repositories.Models
{
	public class RepositoryResponse<T>
	{
		public bool Success { get; set; }

		public T? Data { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public Exception? Exception { get; set; }

		public static RepositoryResponse<T> SuccessFul(T data)
		{
			return new RepositoryResponse<T>
			{
				Success = true,
				Data = data,
				Exception = null
			};
		}

		public static RepositoryResponse<T> Unsuccessful(Exception exception, T data = default(T))
		{
			return new RepositoryResponse<T>
			{
				Success = false,
				Data = data,
				Exception = exception
			};
		}
	}
}
