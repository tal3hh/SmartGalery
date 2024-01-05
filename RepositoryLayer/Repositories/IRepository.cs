using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable GetQueryable();
        void Remove(T entity);
        void Update(T entity, T unchanged);
        Task CreateAsync(T entity);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> exp, bool AsnoTracking = true);
        Task<List<T>> AllIncludeAsync<A>(Expression<Func<T, List<A>>> inclde, Expression<Func<T, int>> exp, bool AscOrDesc = true);
        Task<List<T>> AllIncludeAsync<A>(Expression<Func<T, A>> inclde, Expression<Func<T, int>> exp, bool AscOrDesc = true);
        Task<T> FindAsync(int id);
        Task<List<T>> AllFilterAsync(Expression<Func<T, bool>> exp, bool AsnoTracking = true);
        Task<List<T>> AllOrderByAsync(Expression<Func<T, int>> exp, bool AscOrDesc = true);
        Task<List<T>> AllAsync();
    }
}
