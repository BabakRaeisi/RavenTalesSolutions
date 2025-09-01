using AutoMapper;
using CoreLayer.DTOs;
using CoreLayer.Entities;

namespace CoreLayer.MappingProfiles
{
    public class StoryToStoryResponseMappingProfile : Profile
    {
        public StoryToStoryResponseMappingProfile()
        {
            CreateMap<Story, StoryResponseDto>();
        }
    }
}