using System.Linq.Expressions;
using DataAccess.Contexts;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class RepositoryBase<TEntity>(ApplicationDbContext context) : IRepositoryBase<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext context = context;

    public virtual async Task<ICollection<TEntity>> List()
    {
        return await context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<ICollection<TEntity>?> List(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] include)
    {
        if (predicate == null) return null;

        IQueryable<TEntity> query = context.Set<TEntity>();

        if (include != null)
        {
            foreach (var item in include)
            {
                query = query.Include(item);
            }
        }

        return await query.Where(predicate).ToListAsync();
    }

    public virtual TEntity? GetById(Guid id)
    {
        return context.Set<TEntity>().Find(id);
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await context.Set<TEntity>().FindAsync(id);
    }

    public virtual void Create(TEntity entity)
    {
        context.Set<TEntity>().Add(entity);
    }

    public virtual void Update(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
    }

    public virtual void Delete(Guid id)
    {
        var entity = GetById(id);
        if(entity != null)
        {
            Delete(entity);
        }
    }

    public async Task<bool> Save()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
