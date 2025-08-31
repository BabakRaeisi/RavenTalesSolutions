using AutoMapper;
using CoreLayer.DTOs;
using CoreLayer.Entities;

namespace CoreLayer.MappingProfiles
{
    public class StoryResponseToStoryMappingProfile : Profile
    {
        public StoryResponseToStoryMappingProfile()
        {
            CreateMap<StoryResponseDto, Story>()
                // Only ignore fields that must be set by service logic
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserPreferences, opt => opt.Ignore());
            // All other fields are mapped automatically
        }
    }
}