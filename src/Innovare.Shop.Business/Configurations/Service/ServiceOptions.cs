namespace Innovare.Shop.Business.Configurations.Service
{
    public sealed class ServiceOptions
    {
        public string ApplicationName { get; set; }

        public string ImageUrlBaseAddress { get; set; }

        public string DefaultTenantConnectionString { get; set; }

        public bool IsSmsEnabled { get; set; }

        public string SmsProviderBaseAddress { get; set; }

        public string SmsProviderNumber { get; set; }

        public string SmsProviderUserName { get; set; }

        public string SmsProviderPassword { get; set; }

        public bool IsSejamEnabled { get; set; }

        public string SejamBaseAddress { get; set; }

        public string SejamUserName { get; set; }

        public string SejamPassword { get; set; }

        public string FundAdminUserName { get; set; }

        public string FundAdminUserPassword { get; set; }

        public bool FundAdminRequireAuth { get; set; }
    }
}