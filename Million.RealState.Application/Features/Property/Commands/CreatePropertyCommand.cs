using MediatR;
using Million.RealState.Domain.DTOs;

namespace Million.RealState.Application.Features.Property.Commands;

public record CreatePropertyCommand(PropertyDto Property) :IRequest<ApiResponse<bool>>
{
}
