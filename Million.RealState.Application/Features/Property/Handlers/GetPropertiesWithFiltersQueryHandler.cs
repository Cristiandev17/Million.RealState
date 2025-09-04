using AutoMapper;
using MediatR;
using Million.RealState.Application.Features.Property.Queries;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Domain.Utilities;

namespace Million.RealState.Application.Features.Property.Handlers;

public class GetPropertiesWithFiltersQueryHandler(IPropertyRepository _propertyRepository, IMapper _mapper ) : IRequestHandler<GetPropertiesWithFiltersQuery, ApiResponse<List<PropertyDto>>>
{
    public async Task<ApiResponse<List<PropertyDto>>> Handle(GetPropertiesWithFiltersQuery request, CancellationToken cancellationToken)
    {
       var result = await  _propertyRepository.GetFilteredPropertiesAsync(request.Params);

        if (result == null) 
        {
            return ApiResponse<List<PropertyDto>>.ErrorResult(Constants.NotFound, Constants.ErrorFilters);
        }

        if (!result.Any())
        {
            return ApiResponse<List<PropertyDto>>.ErrorResult(Constants.NotFound, Constants.NoFoundWithFilters);
        }

        var propertiesDto = _mapper.Map<List<PropertyDto>>(result);


        return ApiResponse<List<PropertyDto>>.SuccessResult(propertiesDto, Constants.Success);
    }
}
