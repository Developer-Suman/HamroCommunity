using Microsoft.EntityFrameworkCore.Storage;
using Project.DLL.DbContext;
using Project.DLL.RepoInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed;
        private Hashtable _repositories;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            
        }
        public IDbContextTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if(disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if(_repositories == null )
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if(!_repositories.ContainsKey(type))
            {
                var repositoriesType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoriesType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<TEntity>) _repositories[type];
        }
    }
}
