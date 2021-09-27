using System;

namespace Innovare.Shop.Business.Configurations.Identitiy
{
    public class LockoutOptions
    {
        public TimeSpan DefaultLockoutTimeSpan { get; set; }

        public int MaxFailedAccessAttempts { get; set; }

        public bool AllowedForNewUsers { get; set; }
    }
}