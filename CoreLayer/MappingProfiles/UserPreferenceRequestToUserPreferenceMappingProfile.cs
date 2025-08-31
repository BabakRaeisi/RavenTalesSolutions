using AutoMapper;
using CoreLayer.DTOs;
using CoreLayer.Entities;

namespace CoreLayer.MappingProfiles
{
    public class UserPreferenceRequestToUserPreferenceMappingProfile : Profile
    {
        public UserPreferenceRequestToUserPreferenceMappingProfile()
        {
            CreateMap<UserPreferencesUpdateRequestDto, UserPreferences>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Stories, opt => opt.Ignore());
        }
    }
}