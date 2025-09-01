using CoreLayer.MappingProfiles;
using CoreLayer.Services;
using CoreLayer.StoryContracts;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            // Register business services
            services.AddScoped<IStoryServices, StoryService>();
            services.AddScoped<IUserPreferencesService, UserPreferencesService>();
            
            
            // Register AutoMapper
            services.AddAutoMapper(typeof(StoryRequestToStoryMappingProfile).Assembly);
            
            return services;
        }
    }
}