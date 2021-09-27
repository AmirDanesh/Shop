using Innovare.Shop.Business.Abstractions.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovare.Shop.Business.Services.Common.Database
{
    internal sealed class DatabaseService : ServiceBase, IDatabaseService
    {
        #region Public Constructors

        public DatabaseService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task ArrangeDatabasesAsync()
        {
            await ArrangeDatabaseAsync();
        }

        public async Task ArrangeDatabaseAsync()
        {
            await MigrateDatabaseAsync();
            await SeedDatabaseAsync();
            await CacheDatabaseAsync();
        }

        public async Task MigrateDatabaseAsync()
        {
            try
            {
                await UnitOfWork.MigrateAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task SeedDatabaseAsync()
        {
            try
            {
                await CommitAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task FeedDatabaseWithSampleDataAsync()
        {
            try
            {
                await CommitAsync();
            }
            catch
            {
                throw;
            }
        }

        public Task CacheDatabaseAsync()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}