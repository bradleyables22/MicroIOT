using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.Models;
using Server.Repositories.Extensions;
using Server.Repositories.Models;
using System.Linq.Expressions;

namespace Server.Repositories
{
	public interface IRepository<T> where T : IBaseModel
	{
		Task<RepositoryResponse<List<T>>> GetAll();
		Task<RepositoryResponse<T>> GetById(object id);
		Task<RepositoryResponse<List<T>>> GetWhere(Expression<Func<T, bool>> predicate);
		Task<RepositoryResponse<T>> Create(T entity);
		Task<RepositoryResponse<T>> Update(T entity);
		Task<RepositoryResponse<T>> Delete(object obj);
		Task<RepositoryResponse<T>> Save(T obj);

	}
	public class Repository<T> : IRepository<T> where T : BaseModel
	{
		private readonly AppDbContext _context;
		public Repository(AppDbContext? context)
		{
			_context = context;
		}

		public virtual async Task<RepositoryResponse<T>> Create(T entity)
		{
			await _context.Set<T>().AddAsync(entity);
			var result = await _context.GetSavedChangesAsync(entity);
			return result;
		}

		public virtual async Task<RepositoryResponse<T>> Delete(object obj)
		{
			var entity = await _context.Set<T>().FindAsync(obj);

			if (entity == null)
				return new RepositoryResponse<T>
				{
					Success = false,
					Data = default,
					Exception =
					new KeyNotFoundException($"Entity of type {typeof(T).Name} could not be found.")
				};

			_context.Set<T>().Remove(entity);
			var result = await _context.GetSavedChangesAsync(entity, true);

			return result;
		}

		public virtual async Task<RepositoryResponse<List<T>>> GetAll()
		{
			var result = await _context.Set<T>().ToListAsync().GetResponseAsync();

			return result;
		}

		public virtual async Task<RepositoryResponse<T>> GetById(object id)
		{
			var result = await _context.Set<T>().FindAsync(id).GetResponseAsync();

			return result;
		}
		public virtual async Task<RepositoryResponse<List<T>>> GetWhere(Expression<Func<T, bool>> predicate)
		{
			var result = await _context.Set<T>().Where(predicate).ToListAsync().GetResponseAsync();
			return result;
		}
		public virtual async Task<RepositoryResponse<T>> Save(T obj)
		{
			var result = await _context.GetSavedChangesAsync(obj);
			return result;
		}

		public virtual async Task<RepositoryResponse<T>> Update(T entity)
		{
			_context.Set<T>().Update(entity);
			var result = await _context.GetSavedChangesAsync(entity);
			return result;
		}
	}
}
