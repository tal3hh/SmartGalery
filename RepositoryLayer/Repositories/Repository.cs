using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        readonly DbSet<T> table;
        public Repository(AppDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }


        public async Task<List<T>> AllAsync()
        {
            return await table.AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> AllOrderByAsync(Expression<Func<T, int>> exp, bool AscOrDesc = true)
        {
            return AscOrDesc ? await table.AsNoTracking().OrderBy(exp).ToListAsync() : await table.AsNoTracking().OrderByDescending(exp).ToListAsync();
        }

        public async Task<List<T>> AllIncludeAsync<A>(Expression<Func<T, List<A>>> inclde, Expression<Func<T, int>> exp, bool AscOrDesc = true)
        {
            return AscOrDesc ? await table.AsNoTracking().Include(inclde).OrderBy(exp).ToListAsync() : await table.AsNoTracking().OrderByDescending(exp).ToListAsync();
        }

        public async Task<List<T>> AllIncludeAsync<A>(Expression<Func<T, A>> inclde, Expression<Func<T, int>> exp, bool AscOrDesc = true)
        {
            return AscOrDesc ? await table.AsNoTracking().Include(inclde).OrderBy(exp).ToListAsync() : await table.AsNoTracking().OrderByDescending(exp).ToListAsync();
        }

        public async Task<List<T>> AllFilterAsync(Expression<Func<T, bool>> exp, bool AsnoTracking = true)
        {
            return AsnoTracking ? await table.AsNoTracking().Where(exp).ToListAsync() : await table.Where(exp).ToListAsync();
        }


        public async Task<T> FindAsync(int id)
        {
            return await table.FindAsync(id);
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> exp, bool AsnoTracking = true)
        {
            return AsnoTracking ? await table.AsNoTracking().SingleOrDefaultAsync(exp) : await table.SingleOrDefaultAsync(exp);
        }

        public async Task CreateAsync(T entity)
        {
            await table.AddAsync(entity);
        }

        public void Update(T entity, T unchanged)
        {
            _context.Entry(unchanged).CurrentValues.SetValues(entity);
        }

        public void Remove(T entity)
        {
            table.Remove(entity);
        }

        public IQueryable GetQueryable()
        {
            return table.AsQueryable();
        }
    }
}
