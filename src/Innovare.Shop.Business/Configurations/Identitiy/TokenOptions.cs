using System;

namespace Innovare.Shop.Business.Configurations.Identitiy
{
    public class TokenOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecretKey { get; set; }

        public TimeSpan ExpireDuration { get; set; }
    }
}