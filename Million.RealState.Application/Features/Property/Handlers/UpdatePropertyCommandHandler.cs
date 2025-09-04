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
    public async Task<ApiResponse<bool>> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
    {

        if (request.Property == null)
        {
            return ApiResponse<bool>.ErrorResult(Constants.RequestNull, Constants.InvalidRequest);
        }

        if (request.Property.Id == Guid.Empty)
        {
            return ApiResponse<bool>.ErrorResult(Constants.IdRequired, Constants.ValidError);
        }

        var property = _mapper.Map<PropertyEntity>(request.Property);

        await _propertyRepository.UpdateProperty(property);        

        return ApiResponse<bool>.SuccessResult(true, Constants.SavedProperty);
    }
}
