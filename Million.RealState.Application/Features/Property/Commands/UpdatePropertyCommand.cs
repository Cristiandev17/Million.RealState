using MediatR;
using Million.RealState.Domain.DTOs;

namespace Million.RealState.Application.Features.Property.Commands;

public record UpdatePropertyCommand(PropertyDto Property) : IRequest<ApiResponse<bool>>
{
}
