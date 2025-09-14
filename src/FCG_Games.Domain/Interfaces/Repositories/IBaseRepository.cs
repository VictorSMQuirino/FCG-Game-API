using FCG_Games.Domain.Entities;
using System.Linq.Expressions;

namespace FCG_Games.Domain.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
	Task<T?> GetByIdAsync(Guid id);
	Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);
	Task<ICollection<T>> GetAllAsync();
	Task<ICollection<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
	Task<ICollection<T>> GetListBy(Expression<Func<T, bool>> predicate);
	Task<ICollection<T>> GetListBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
	Task CreateAsync(T entity);
	Task UpdateAsync(T entity);
	Task DeleteAsync(T entity);
	Task<bool> ExistsBy(Expression<Func<T, bool>> predicate);
	Task<T?> GetBy(Expression<Func<T, bool>> predicate);
}
