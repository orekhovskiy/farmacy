using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApi.Models;
using UserApi.ViewModels;
using AutoMapper;

namespace UserApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.PositionNavigation.Name))
                .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.Position));
        }
    }
}
