using Innovare.Shop.Data.DbContexts;

namespace Innovare.Shop.Data.Abstractions.Repository
{
    internal abstract class RepositoryBase<TModel> : IRepositoryBase<TModel> where TModel : InnovareShopModelBase
    {
        protected RepositoryBase(ShopDbContext dbContext)
        {
        }
    }
}