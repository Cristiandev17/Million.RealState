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
    public async Task<ApiResponse<bool>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        if (request.Property == null)
        {
            return ApiResponse<bool>.ErrorResult(Constants.RequestNull, Constants.InvalidRequest);
        }

        if (string.IsNullOrEmpty(request.Property.OwnerId.ToString()))
        {
            return ApiResponse<bool>.ErrorResult(Constants.RequiredOwnerId, Constants.ValidError);
        }

        var property = _mapper.Map<PropertyEntity>(request.Property);        

        var propertyId = await _propertyRepository.CreatePropertyAsync(property);        

        return ApiResponse<bool>.SuccessResult(true, Constants.SavedProperty);
    }
}
