using AutoMapper;
using CoreLayer.DTOs;
using CoreLayer.Entities;

namespace CoreLayer.MappingProfiles
{
    public sealed class UserPreferenceRequestToUserPreferenceMappingProfile : Profile
    {
        public UserPreferenceRequestToUserPreferenceMappingProfile()
        {
            CreateMap<UserPreferencesUpdateRequestDto, UserPreferences>()
                .ForMember(d => d.LastUsedLanguageLevel, opt => opt.MapFrom(s => s.LanguageLevel))
                .ForMember(d => d.LastUsedTargetLanguage, opt => opt.MapFrom(s => s.TargetLanguage))
                .ForMember(d => d.PreferredTranslationLanguage, opt => opt.MapFrom(s => s.PreferredTranslationLanguage))
                .ForMember(d => d.SeenStoryIds, opt => opt.Ignore())
                .ForMember(d => d.SavedStoryIds, opt => opt.Ignore())
                .ForMember(d => d.UserId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());
        }
    }
}