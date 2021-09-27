using System;

namespace Innovare.Shop.Business.Abstractions.Service
{
    public interface IServiceBase : IDisposable
    {
        //Task SetTenantAsync(Guid tenantId);
        void SetAsPrimary();

        void SetAsAuxiliary();
    }
}