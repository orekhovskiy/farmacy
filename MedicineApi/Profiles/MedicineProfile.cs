using AutoMapper;
using Farmacy.Models;
using Farmacy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicineApi.Profiles
{
    public class MedicineProfile : Profile
    {
        public MedicineProfile()
        {
            CreateMap<Medicine, MedicineViewModel>()
                .ForMember(dest => dest.Form, opt => opt.MapFrom(src => src.Form.Name))
                .ForMember(dest => dest.Producer, opt => opt.MapFrom(src => src.Producer.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Components, opt => opt.MapFrom(src => src.MedicineComposition.Select(mc => mc.Component.Name)));
        }
    }
}
