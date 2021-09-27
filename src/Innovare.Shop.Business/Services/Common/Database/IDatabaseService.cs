using Innovare.Shop.Business.Abstractions.Service;
using System.Threading.Tasks;

namespace Innovare.Shop.Business.Services.Common.Database
{
    internal interface IDatabaseService : IServiceBase
    {
        Task ArrangeDatabaseAsync();

        Task ArrangeDatabasesAsync();

        Task CacheDatabaseAsync();

        Task MigrateDatabaseAsync();

        Task SeedDatabaseAsync();

        Task FeedDatabaseWithSampleDataAsync();
    }
}