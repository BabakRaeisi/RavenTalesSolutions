using RavenTales.Core.RepositoryContracts;
using RavenTales.Core.ServiceContracts;
using RavenTales.Infrastructure.DbContext;
using RavenTales.Infrastructure.Repositories;
using RavenTales.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
 

namespace RavenTales.Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Extension method to add infrastructure services to the dependecy injection container.
        /// 
        ///</summary>
        ///>/// <param name="services">.</param>
        ///<returns></returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register your infrastructure services here
            // Example: services.AddSingleton<IMyService, MyService>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<DapperDbContext>();

            // Add this line to register TokenService
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
