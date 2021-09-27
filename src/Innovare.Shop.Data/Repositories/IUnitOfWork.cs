using System;
using System.Threading.Tasks;

namespace Innovare.Shop.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task MigrateAsync();

        Task<int> SaveChangesAsync();

        void SetConnectionString(string connectionString);
    }
}