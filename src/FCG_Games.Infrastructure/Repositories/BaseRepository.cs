using FCG_Games.Domain.Entities;
using FCG_Games.Domain.Interfaces.Repositories;
using FCG_Games.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace FCG_Games.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
	private readonly FCGGamesDbContext _context;
	private readonly DbSet<T> _dbSet;

	public BaseRepository(FCGGamesDbContext context)
	{
		_context = context;
		_dbSet = _context.Set<T>();
	}

	public async Task CreateAsync(T entity)
	{
		await _dbSet.AddAsync(entity);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync(T entity)
	{
		entity.UpdatedAt = DateTime.UtcNow;
		_dbSet.Update(entity);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(T entity)
	{
		_dbSet.Remove(entity);
		await _context.SaveChangesAsync();
	}

	public async Task<T?> GetByIdAsync(Guid id)
		=> await _dbSet.FindAsync(id);

	public async Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
	{
		var query = _dbSet.AsQueryable();

		includes.ToList().ForEach(i => query.Include(i));

		return await query.SingleOrDefaultAsync(e => e.Id == id);
	}

	public async Task<ICollection<T>> GetAllAsync()
		=> await _dbSet.ToListAsync();

	public async Task<ICollection<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
	{
		var query = _dbSet.AsQueryable();

		includes.ToList().ForEach(i => query.Include(i));

		return await query.ToListAsync();
	}

	public async Task<ICollection<T>> GetListBy(Expression<Func<T, bool>> predicate)
		=> await _dbSet.Where(predicate).ToListAsync();

	public async Task<ICollection<T>> GetListBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
	{
		var query = _dbSet.AsQueryable();

		includes.ToList().ForEach(i => query.Include(i));

		return await query.Where(predicate).ToListAsync();
	}

	public async Task<T?> GetBy(Expression<Func<T, bool>> predicate)
		=> await _dbSet.Where(predicate).FirstOrDefaultAsync();

	public async Task<bool> ExistsBy(Expression<Func<T, bool>> predicate)
		=> await _dbSet.AnyAsync(predicate);

	public async Task<IDbContextTransaction> BeginTransaction()
		=> await _context.Database.BeginTransactionAsync();
}
