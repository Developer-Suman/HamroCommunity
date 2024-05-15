using Microsoft.EntityFrameworkCore;
using Project.DLL.DbContext;
using Project.DLL.RepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Repository.Implementation
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
       private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            
        }
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRange(List<TEntity> entity)
        {
            await _dbSet.AddRangeAsync(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task DeleteAllAsync()
        {
            var allData = await _dbSet.ToListAsync();
            _dbSet.RemoveRange(allData);
        }

        public void DeleteRange(List<TEntity> entity)
        {
            _dbSet.RemoveRange(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(string id) => await _dbSet.FindAsync(id);
        

        public async Task<IEnumerable<TEntity>> GetConditonalAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate) => await _dbSet.SingleOrDefaultAsync(predicate);
       

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
