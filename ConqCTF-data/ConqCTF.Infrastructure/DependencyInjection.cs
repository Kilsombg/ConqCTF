using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Infrastructure.Data;
using ConqCTF.Infrastructure.Data.Interceptors;
using ConqCTF.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConqCTF.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("ConqCTFDB");

            builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

                options.UseSqlServer(connectionString);
            });

            builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            builder.Services.AddScoped<ApplicationDbContextInitialiser>();

            builder.Services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddSingleton(TimeProvider.System);
            builder.Services.AddTransient<IIdentityService, IdentityService>();

            /*builder.Services.AddAuthorization(options =>
                options.AddPolicy(Policies., policy => policy.RequireRole(Roles.Administrator)));*/
        }
    };
}
