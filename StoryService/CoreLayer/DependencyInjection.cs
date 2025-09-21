using CoreLayer.MappingProfiles;
using CoreLayer.Services;
using CoreLayer.ServiceContracts;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            // Register business services
            services.AddScoped<IStoryServices, StoryService>();
          
            
            
            // Register AutoMapper
            services.AddAutoMapper(typeof(StoryToStoryResponseMappingProfile).Assembly);
            
            return services;
        }
    }
}