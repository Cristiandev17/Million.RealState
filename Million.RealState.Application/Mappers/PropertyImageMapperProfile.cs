using AutoMapper;

namespace Million.RealState.Application.Mappers;

public class PropertyImageMapperProfile : Profile
{
    public PropertyImageMapperProfile()
    {
        // Property mappings
        CreateMap<Domain.Entities.PropertyImageEntity, Domain.DTOs.PropertyImageDto>()
            .ReverseMap();

        CreateMap<Domain.DTOs.PropertyImageDto, Domain.Entities.PropertyImageEntity>()
            .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdateDate, opt => opt.Ignore());
    }
}
