using AutoMapper;
using MediatR;
using Million.RealState.Application.Features.Property.Commands;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Entities;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Domain.Utilities;

namespace Million.RealState.Application.Features.Property.Handlers;

public class UpdatePropertyCommandHandler(
    IPropertyRepository _propertyRepository,   
    IMapper _mapper) : IRequestHandler<UpdatePropertyCommand, ApiResponse<bool>>
{

    // Handles the property update command
    public async Task<ApiResponse<bool>> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
    {
        // Validate that the property data is not null
        if (request.Property == null)
        {
            return ApiResponse<bool>.ErrorResult(Constants.RequestNull, Constants.InvalidRequest);
        }

        // Validate that the property has a valid GUID identifier
        if (request.Property.Id == Guid.Empty)
        {
            return ApiResponse<bool>.ErrorResult(Constants.IdRequired, Constants.ValidError);
        }

        // Map the DTO to PropertyEntity using AutoMapper
        var property = _mapper.Map<PropertyEntity>(request.Property);

        // Update the property in the database through the repository
        await _propertyRepository.UpdateProperty(property);        

        return ApiResponse<bool>.SuccessResult(true, Constants.SavedProperty);
    }
}
