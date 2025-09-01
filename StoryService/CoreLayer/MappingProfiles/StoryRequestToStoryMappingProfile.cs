using AutoMapper;
using CoreLayer.DTOs;
using CoreLayer.Entities;

namespace CoreLayer.MappingProfiles
{
    public class StoryRequestToStoryMappingProfile : Profile
    {
        public StoryRequestToStoryMappingProfile()
        {
            CreateMap<StoryRequestDto, Story>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsBookmarked, opt => opt.Ignore());
        }
    }
}