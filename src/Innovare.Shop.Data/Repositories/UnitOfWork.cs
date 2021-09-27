using System;
using System.Threading.Tasks;
using Innovare.Shop.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Innovare.Shop.Data.Repositories
{
    public abstract class UnitOfWork<TDbContext> : IUnitOfWork
        where TDbContext : DbContext, IShopDbContext
    {
        protected UnitOfWork(TDbContext dbContext, IServiceProvider serviceProvider)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            isDisposed = false;
        }

        protected TDbContext DbContext { get; }

        protected IServiceProvider ServiceProvider { get; }
        protected bool isDisposed;

        public virtual async Task<int> SaveChangesAsync()
        {
            var rowAffected = 0;

            if (DbContext.ChangeTracker.HasChanges())
            {
                rowAffected = await DbContext.SaveChangesAsync();
            }

            return rowAffected;
        }

        public virtual void Dispose()
        {
            if (!isDisposed)
            {
                DbContext.Dispose();
                isDisposed = true;

                GC.SuppressFinalize(this);
                GC.Collect();
            }
        }

        public virtual async Task MigrateAsync()
        {
            await DbContext.Database.MigrateAsync();
        }

        public virtual void SetConnectionString(string connectionString)
        {
            DbContext.SetConnectionString(connectionString);
            DbContext.ChangeTracker.Clear();
        }

        protected virtual TRepository GetRepository<TRepository>()
        {
            var repository = ServiceProvider.GetService<TRepository>();

            return repository;
        }
    }
}