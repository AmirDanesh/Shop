using AutoMapper;
using Innovare.Shop.Business.Configurations.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Security.Claims;
using Innovare.Shop.Data.Repositories;
using Innovare.Shop.Business.Dtos.V00_00.Identity;
using Innovare.Shop.Business.Dtos.Common;
using Innovare.Shop.Business.Exceptions;
using System.Text.Json;

namespace Innovare.Shop.Business.Abstractions.Service
{
    internal abstract class ServiceBase : IServiceBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDistributedCache _cache;
        private bool _isDisposed;

        internal ServiceBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _isDisposed = false;

            UnitOfWork = _serviceProvider.GetService<IUnitOfWork>() ?? throw new ArgumentNullException(nameof(UnitOfWork));

            Mapper = _serviceProvider.GetService<IMapper>() ?? throw new ArgumentNullException(nameof(Mapper));
            HttpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>() ?? throw new ArgumentNullException(nameof(HttpContextAccessor));

            DomainName = GetType().Name.Replace("Service", string.Empty);
            ServiceOptions = _serviceProvider.GetService<IOptions<ServiceOptions>>()?.Value ?? throw new ArgumentNullException(nameof(ServiceOptions));
            DatabaseFeedsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DatabaseFiles", Guid.Empty.ToString(), DomainName);
            BusinessValidationErrors = new Dictionary<string, List<string>>();

            _httpClientFactory = _serviceProvider.GetService<IHttpClientFactory>() ?? throw new ArgumentNullException(nameof(IHttpClientFactory));
            _cache = _serviceProvider.GetService<IDistributedCache>() ?? throw new ArgumentNullException(nameof(IDistributedCache));

            SetCurrentUser();
            SetAsPrimary();
        }

        private void SetCurrentUser()
        {
            try
            {
                if ((HttpContextAccessor?.HttpContext?.User?.Claims?.Any() ?? false)
                    && HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    CurrentUser = new CurrentUserDto()
                    {
                        FirstName = HttpContextAccessor.HttpContext.User.Claims.Single(claim => claim.Type.Equals(ClaimTypes.GivenName)).Value,
                        LastName = HttpContextAccessor.HttpContext.User.Claims.Single(claim => claim.Type.Equals(ClaimTypes.Surname)).Value,
                        FullName = HttpContextAccessor.HttpContext.User.Claims.Single(claim => claim.Type.Equals(ClaimTypes.Name)).Value,
                        Role = HttpContextAccessor.HttpContext.User.Claims.Single(claim => claim.Type.Equals(ClaimTypes.Role)).Value,
                        NationalCode = HttpContextAccessor.HttpContext.User.Claims.Single(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value
                    };
                }
                else
                {
                    CurrentUser = null;
                }
            }
            catch
            {
                throw;
            }
        }

        protected IUnitOfWork UnitOfWork { get; private set; }

        protected IMapper Mapper { get; private set; }

        protected IHttpContextAccessor HttpContextAccessor { get; private set; }

        protected IDictionary<string, List<string>> BusinessValidationErrors { get; private set; }

        public string DomainName { get; private set; }

        public ServiceOptions ServiceOptions { get; private set; }

        public CurrentUserDto CurrentUser { get; private set; }

        public bool IsUserAuthenticated => CurrentUser is not null;

        public string DatabaseFilePath
        {
            get
            {
                var databaseFeedsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DatabaseFiles", DomainName);
                return databaseFeedsFilePath;
            }
        }

        public string DatabaseFeedsFilePath { get; }

        public bool IsPrimary { get; private set; }

        public virtual void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
            }

            GC.SuppressFinalize(this);
        }

        public virtual void SetAsPrimary()
        {
            IsPrimary = true;
        }

        public virtual void SetAsAuxiliary()
        {
            IsPrimary = false;
        }

        protected async Task<int?> CommitAsync()
        {
            try
            {
                int? rowAffected = null;

                if (IsPrimary)
                {
                    rowAffected = await UnitOfWork.SaveChangesAsync();
                }

                return rowAffected;
            }
            catch
            {
                throw;
            }
        }

        protected async Task<bool> CommitAndReturnChangeResultAsync()
        {
            var rowAffected = await CommitAsync();
            var isSaveChangesSuccessful = IsSaveChangesSuccessful(rowAffected);

            return isSaveChangesSuccessful;
        }

        private bool IsSaveChangesSuccessful(int? rowAffected)
        {
            var isSaveChangeHasAffectedRows = !rowAffected.HasValue || rowAffected.Value > 0;

            return isSaveChangeHasAffectedRows;
        }

        protected string SaveFile(byte[] file, string fileName, string currentFileName = null, int maxImageWidth = 1920)
        {
            //return FileHelper.SaveFile(file, DatabaseFilePath, DomainName, fileName, currentFileName, maxImageWidth);
            throw new NotImplementedException();
        }

        protected void RemoveFile(string fileName)
        {
            //FileHelper.RemoveFile(DatabaseFilePath, fileName);
            throw new NotImplementedException();
        }

        protected void AddValidationError(string message)
        {
            AddValidationError(string.Empty, message);
        }

        protected void AddValidationError(string key, string message)
        {
            var isKeyExists = BusinessValidationErrors.ContainsKey(key);

            if (isKeyExists)
            {
                var error = BusinessValidationErrors.SingleOrDefault(model => model.Key.Equals(key, StringComparison.OrdinalIgnoreCase));

                error.Value.Add(message);
            }
            else
            {
                BusinessValidationErrors.Add(key, new List<string>() { message });
            }
        }

        protected void ThrowBusinessValidationException()
        {
            if (BusinessValidationErrors.Any())
            {
                var keyPairValueList = BusinessValidationErrors
                    .Select(model => new KeyValuePair<string, string[]>(model.Key, model.Value.ToArray()))
                    .ToList();

                var errorDictionary = new Dictionary<string, string[]>(keyPairValueList);
                var errorDictionaryJson = JsonSerializer.Serialize(errorDictionary);

                throw new BusinessValidationException(errorDictionaryJson);
            }
        }

        protected ImageDto GenerateImageMetadata(string filename, string alternativeText)
        {
            filename = filename.ToLower();

            var imageDto = new ImageDto()
            {
                AlternativeText = alternativeText,
                Url = GenerateFileUrl(filename),
                FileName = Path.GetFileNameWithoutExtension(filename),
                FileExtension = Path.GetExtension(filename).Replace(".", string.Empty),
                FileFullName = filename
            };

            return imageDto;
        }

        protected ImageDto GenerateImageThumbnailMetadata(string filename, string alternativeText)
        {
            filename = $"thumb_{filename}".ToLower();

            var imageDto = new ImageDto()
            {
                AlternativeText = alternativeText,
                Url = GenerateFileUrl(filename),
                FileName = Path.GetFileNameWithoutExtension(filename),
                FileExtension = Path.GetExtension(filename).Replace(".", string.Empty),
                FileFullName = filename
            };

            return imageDto;
        }

        protected string GenerateFileUrl(string filename)
        {
            //var url = $"{ServiceOptions.ImageUrlBaseAddress}/{TenantManagerService.TenantInfo.TenantId}/{ObjectName}/{filename}".ToLower();

            //return url;
            throw new NotImplementedException();
        }
    }
}