using AutoMapper;
using CoreLayer.Entities;
using RavenTales.Shared;
using System;

namespace CoreLayer.MappingProfiles
{
    public class StoryToStoryCardMappingProfile : Profile
    {
        public StoryToStoryCardMappingProfile()
        {
            CreateMap<Story, StoryCardDto>()
                .ForMember(dest => dest.StoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.LanguageLevel, opt => opt.MapFrom(src => src.LanguageLevel))
                .ForMember(dest => dest.TargetLanguage, opt => opt.MapFrom(src => src.StoryLanguage))
                .ForMember(dest => dest.Topic, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.TextPreview, opt => opt.MapFrom(src => 
                    src.Content.Length > 200 ? src.Content.Substring(0, 197) + "..." : src.Content));
        }
    }
}