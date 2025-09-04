using MediatR;
using Million.RealState.Application.Features.Property.Commands;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Domain.Utilities;

namespace Million.RealState.Application.Features.Property.Handlers;

public class UpdatePriceOfPropertyCommandHandler(IPropertyRepository _propertyRepository) : IRequestHandler<UpdatePriceOfPropertyCommand, ApiResponse<bool>>
{
    // Handler for updating property price
    public async Task<ApiResponse<bool>> Handle(UpdatePriceOfPropertyCommand request, CancellationToken cancellationToken)
    {
        // Validate that property ID is provided
        if (string.IsNullOrEmpty(request.PropertyId))
        {
            return ApiResponse<bool>.ErrorResult(Constants.IdRequired, Constants.InvalidRequest);
        }

        // Validate that price is greater than zero
        if (request.Price == 0 || request.Price <= 0)
        {
            return ApiResponse<bool>.ErrorResult(Constants.PriceNoZero, Constants.ValidError);
        }

        // Update the price in the repository
        await _propertyRepository.UpdatePriceOfProperty( new Guid(request.PropertyId), request.Price);

        return ApiResponse<bool>.SuccessResult(true, Constants.UpdatedPrice);
    }
}
