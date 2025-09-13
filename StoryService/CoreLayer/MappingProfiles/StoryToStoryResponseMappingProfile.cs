using AutoMapper;
using CoreLayer.DTOs;
using CoreLayer.Entities;

namespace CoreLayer.MappingProfiles
{
    public sealed class StoryToStoryResponseMappingProfile : Profile
    {
        public StoryToStoryResponseMappingProfile()
        {
            // leaf types first
            CreateMap<Segment, SegmentDto>();
            CreateMap<Unit, UnitDto>();
            CreateMap<Sentence, SentenceDto>();

            // root type last
            CreateMap<Story, StoryResponseDto>();
        }
    }
    
    
}