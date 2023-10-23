using BookListing.Application.Common.Interfaces;
using BookListing.Domain.Common;
using BookListing.Domain.Identity;
using BookListing.Infrastructure.Data;
using BookListing.Infrastructure.Helpers;
using BookListing.Infrastructure.Identity;
using BookListing.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BookListing.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services
                .AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<User, ApplicationDbContext>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();


            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);


            // Register AppSettings instance
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.AddSingleton(appSettings);            // configure jwt authentication

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

            }).AddIdentityServerJwt();


            //Policy for managing users
            services.AddAuthorization(options =>
                options.AddPolicy("UserManagement", policy => policy.RequireRole("Admin")));

            //Policy for managing deparments or categories
            services.AddAuthorization(options =>
                options.AddPolicy("DepartmentsManagement", policy => policy.RequireRole("Admin")));

            //Policy for Reporting
            services.AddAuthorization(options =>
                options.AddPolicy("Reporting", policy => policy.RequireRole("Admin")));

            return services;
        }
    }
}
