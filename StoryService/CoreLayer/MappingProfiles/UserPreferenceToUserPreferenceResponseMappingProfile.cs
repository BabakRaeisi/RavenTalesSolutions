using AutoMapper;
using CoreLayer.DTOs;
using CoreLayer.Entities;

namespace CoreLayer.MappingProfiles
{
    public sealed class UserPreferenceToUserPreferenceResponseMappingProfile : Profile
    {
        public UserPreferenceToUserPreferenceResponseMappingProfile()
        {
            CreateMap<UserPreferences, UserPreferencesResponseDto>();
        }
    }
}