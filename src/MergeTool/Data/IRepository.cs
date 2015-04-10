using System;
using System.Linq;

namespace MergeTool.Data
{
    public interface IRepository<TEntity>
    {
        TEntity Save(TEntity entity);
        IQueryable<TEntity> Find(Func<TEntity, bool> predicate);
        IQueryable<TEntity> FindAll();
    }
}
