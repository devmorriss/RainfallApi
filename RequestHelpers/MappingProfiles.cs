using AutoMapper;
using RainfallApi.Contracts;
using RainfallApi.DTOs;

namespace RainfallApi.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Item, RainfallReading>()
            .ForMember(dest => dest.AmountMeasured, o => o.MapFrom(i => i.Value))
            .ForMember(dest => dest.DateMeasured, o => o.MapFrom(i => i.DateTime));
    }
}
