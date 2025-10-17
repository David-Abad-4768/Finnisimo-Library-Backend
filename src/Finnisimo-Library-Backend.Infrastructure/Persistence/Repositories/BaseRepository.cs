using Microsoft.EntityFrameworkCore;
using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    where TEntity : BaseEntity
{
  protected readonly ApplicationDbContext _dbContext;
  protected readonly DbSet<TEntity> _dbSet;

  public BaseRepository(ApplicationDbContext dbContext)
  {
    _dbContext = dbContext;
    _dbSet = _dbContext.Set<TEntity>();
  }

  public virtual async Task<TEntity?>
  GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
  {
    return await _dbSet.FindAsync([id], cancellationToken);
  }

  public virtual async Task<IReadOnlyList<TEntity>>
  ListAsync(CancellationToken cancellationToken = default)
  {
    return await _dbSet.ToListAsync(cancellationToken);
  }

  public virtual async Task
  AddAsync(TEntity entity, CancellationToken cancellationToken = default)
  {
    await _dbSet.AddAsync(entity, cancellationToken);
  }

  public virtual Task
  UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
  {
    _dbSet.Update(entity);
    return Task.CompletedTask;
  }

  public virtual Task
  DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
  {
    _dbSet.Remove(entity);
    return Task.CompletedTask;
  }
}
