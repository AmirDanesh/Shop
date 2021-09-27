using Innovare.Shop.Data.DbContexts;
using Innovare.Shop.Data.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Innovare.Shop.Business.Configurations.Identitiy
{
    public static class IdentityConfiguration
    {
        public static void AddIdentity(this IServiceCollection serviceCollection)
        {
            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var configurationSection = serviceProvider.GetService<IConfiguration>().GetSection("identity");
                var identityOptions = configurationSection.Get<IdentityOptions>();

                if (identityOptions is not null && identityOptions.IsEnabled)
                {
                    serviceCollection
                        .AddIdentity<UserModel, RoleModel>(options =>
                        {
                            options.User.RequireUniqueEmail = identityOptions.User.RequireUniqueEmail;
                            options.Password.RequireDigit = identityOptions.Password.RequireDigit;
                            options.Password.RequiredLength = identityOptions.Password.RequiredLength;
                            options.Password.RequireNonAlphanumeric = identityOptions.Password.RequireNonAlphanumeric;
                            options.Password.RequireUppercase = identityOptions.Password.RequireUppercase;
                            options.Password.RequireLowercase = identityOptions.Password.RequireLowercase;
                            options.Password.RequiredUniqueChars = identityOptions.Password.RequiredUniqueChars;
                            options.Lockout.DefaultLockoutTimeSpan = identityOptions.Lockout.DefaultLockoutTimeSpan;
                            options.Lockout.MaxFailedAccessAttempts = identityOptions.Lockout.MaxFailedAccessAttempts;
                            options.Lockout.AllowedForNewUsers = identityOptions.Lockout.AllowedForNewUsers;
                            options.SignIn.RequireConfirmedAccount = identityOptions.SignIn.RequireConfirmedAccount;
                            options.SignIn.RequireConfirmedEmail = identityOptions.SignIn.RequireConfirmedEmail;
                            options.SignIn.RequireConfirmedPhoneNumber = identityOptions.SignIn.RequireConfirmedPhoneNumber;
                        })
                        .AddRoles<RoleModel>()
                        .AddEntityFrameworkStores<ShopDbContext>()
                        .AddDefaultTokenProviders();

                    serviceCollection
                        .AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                        {
                            var secret = Encoding.UTF8.GetBytes(identityOptions.Token.SecretKey);
                            var key = new SymmetricSecurityKey(secret);

                            options.TokenValidationParameters = new TokenValidationParameters()
                            {
                                ValidateIssuer = true,
                                ValidIssuer = identityOptions.Token.Issuer,
                                ValidateAudience = true,
                                ValidAudience = identityOptions.Token.Audience,
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = key,
                                RequireExpirationTime = true,
                            };
                        });

                    serviceCollection.AddAuthorization(options =>
                    {
                        //options.AddPolicy("SuperAdministrator", policy => policy.RequireRole("SuperAdministrator"));
                        //options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
                        //options.AddPolicy("User", policy => policy.RequireRole("User"));
                    });

                    serviceCollection.Configure<IdentityOptions>(configurationSection);
                }
            }
        }

        public static void UseIdentity(this IApplicationBuilder applicationBuilder)
        {
            var databaseContextOptions = applicationBuilder.ApplicationServices.GetService<IConfiguration>().GetSection("identity").Get<IdentityOptions>();

            if (databaseContextOptions is not null && databaseContextOptions.IsEnabled)
            {
                applicationBuilder.UseRouting();
                applicationBuilder.UseAuthentication();
                applicationBuilder.UseAuthorization();
            }
        }
    }
}