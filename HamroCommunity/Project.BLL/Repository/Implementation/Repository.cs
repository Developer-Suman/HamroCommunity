﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<IQueryable<TEntity>> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var result = _dbSet.Where(predicate);
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while finding entities.");
                throw;
            }
        }




        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public Task<IQueryable<TEntity>> GetAllAsyncWithPagination()
        {
            return Task.FromResult(_dbSet.AsNoTracking().AsQueryable());
        }

        public Task<IQueryable<TEntity>> GetAllDataAsync()
        {
            return Task.FromResult(_dbSet.AsQueryable());
        }

        public async Task<TEntity> GetById(int id) => await _dbSet.FindAsync(id);
        

        public async Task<TEntity> GetByIdAsync(string id) => await _dbSet.FindAsync(id);
        

        public async Task<IEnumerable<TEntity>> GetConditonalAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<List<TEntity>> GetFilterAndOrderByAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if(predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderby != null)
            {
                query = orderby(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate) => await _dbSet.SingleOrDefaultAsync(predicate);
       

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
