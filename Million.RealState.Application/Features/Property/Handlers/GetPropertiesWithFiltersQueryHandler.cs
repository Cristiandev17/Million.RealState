using AutoMapper;
using MediatR;
using Million.RealState.Application.Features.Property.Queries;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Domain.Utilities;

namespace Million.RealState.Application.Features.Property.Handlers;

public class GetPropertiesWithFiltersQueryHandler(IPropertyRepository _propertyRepository, IMapper _mapper ) : IRequestHandler<GetPropertiesWithFiltersQuery, ApiResponse<List<PropertyDto>>>
{
    // Retrieves properties based on filter parameters
    public async Task<ApiResponse<List<PropertyDto>>> Handle(GetPropertiesWithFiltersQuery request, CancellationToken cancellationToken)
    {
        // Retrieve properties from repository using the provided filter parameters
        var result = await  _propertyRepository.GetFilteredPropertiesAsync(request.Params);

        // Check if the result is null (unexpected error scenario)
        if (result == null) 
        {
            return ApiResponse<List<PropertyDto>>.ErrorResult(Constants.NotFound, Constants.ErrorFilters);
        }

        // Check if any properties were found matching the filter criteria
        if (!result.Any())
        {
            return ApiResponse<List<PropertyDto>>.ErrorResult(Constants.NotFound, Constants.NoFoundWithFilters);
        }

        // Map the entity list to DTO list using AutoMapper
        var propertiesDto = _mapper.Map<List<PropertyDto>>(result);

        // Return successful response with the mapped DTO list
        return ApiResponse<List<PropertyDto>>.SuccessResult(propertiesDto, Constants.Success);
    }
}
