namespace Finnisimo_Library_Backend.Domain.Abstractions;

public interface IBaseRepository<TEntity, TKey>
    where TEntity : BaseEntity
{
  Task<TEntity?> GetByIdAsync(TKey id,
                              CancellationToken cancellationToken = default);

  Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

  Task UpdateAsync(TEntity entity,
                   CancellationToken cancellationToken = default);

  Task DeleteAsync(TEntity entity,
                   CancellationToken cancellationToken = default);

  Task<IReadOnlyList<TEntity>>
  ListAsync(CancellationToken cancellationToken = default);
}
