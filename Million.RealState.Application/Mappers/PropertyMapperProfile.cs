using AutoMapper;

namespace Million.RealState.Application.Mappers;

public class PropertyMapperProfile : Profile
{
    public PropertyMapperProfile()
    {
        // Property mappings
        CreateMap<Domain.Entities.PropertyEntity, Domain.DTOs.PropertyDto>()
            .ReverseMap();

        CreateMap<Domain.DTOs.PropertyDto, Domain.Entities.PropertyEntity>()          
            .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdateDate, opt => opt.Ignore());
    }
}
