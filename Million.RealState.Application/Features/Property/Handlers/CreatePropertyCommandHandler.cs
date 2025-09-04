using AutoMapper;
using MediatR;
using Million.RealState.Application.Features.Property.Commands;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Entities;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Domain.Utilities;

namespace Million.RealState.Application.Features.Property.Handlers;

public class CreatePropertyCommandHandler(
    IPropertyRepository _propertyRepository,     
    IMapper _mapper) : IRequestHandler<CreatePropertyCommand, ApiResponse<bool>>
{
    // Handles the property creation command
    public async Task<ApiResponse<bool>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        // Validate that the property data is not null
        if (request.Property == null)
        {
            return ApiResponse<bool>.ErrorResult(Constants.RequestNull, Constants.InvalidRequest);
        }

        // Validate that the property has an associated owner ID
        if (string.IsNullOrEmpty(request.Property.OwnerId.ToString()))
        {
            return ApiResponse<bool>.ErrorResult(Constants.RequiredOwnerId, Constants.ValidError);
        }

        // Map the PropertyDto to PropertyEntity using AutoMapper
        var property = _mapper.Map<PropertyEntity>(request.Property);

        // Create the property in the database and get the generated ID
        var propertyId = await _propertyRepository.CreatePropertyAsync(property);

        // Return success response with confirmation message
        return ApiResponse<bool>.SuccessResult(true, Constants.SavedProperty);
    }
}
