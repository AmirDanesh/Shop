using System;

namespace Innovare.Shop.Data.Abstractions
{
    public abstract class InnovareShopModelBase
    {
        public Guid Id { get; set; }

        public byte[] RowVersion { get; set; }
    }
}