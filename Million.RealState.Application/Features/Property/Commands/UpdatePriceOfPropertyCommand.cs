using MediatR;
using Million.RealState.Domain.DTOs;

namespace Million.RealState.Application.Features.Property.Commands;

public record UpdatePriceOfPropertyCommand(string PropertyId, decimal Price) : IRequest<ApiResponse<bool>>
{
}
