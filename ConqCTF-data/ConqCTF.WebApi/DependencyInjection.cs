using ConqCTF.Application.Common.Interfaces;
using ConqCTF.WebApi.Infrastructure;
using ConqCTF.WebApi.Services;

namespace ConqCTF.WebApi
{
    public static class DependencyInjection
    {
        public static void AddWebServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IUser, CurrentUser>();


            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        }
    }
}
