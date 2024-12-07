using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    Task<ICollection<TEntity>> GetAll();
    Task<ICollection<TEntity>?> GetAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] include);
    TEntity? GetById(Guid id);
    Task<TEntity?> GetByIdAsync(Guid id);
    void Create(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void Delete(Guid id);
    Task<bool> Save();
}
