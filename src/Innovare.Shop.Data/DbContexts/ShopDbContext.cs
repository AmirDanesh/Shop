using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Innovare.Shop.Data.DbContexts
{
    public sealed class ShopDbContext : DbContext, IShopDbContext
    {
        private readonly Assembly _configurationsAssembly;

        public ShopDbContext(DbContextOptions options, Assembly configurationsAssembly) : base(options)
        {
            _configurationsAssembly = configurationsAssembly ?? throw new ArgumentNullException(nameof(configurationsAssembly));
        }

        public void SetConnectionString(string connectionString)
        {
            Database.SetConnectionString(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(_configurationsAssembly);
        }
    }
}