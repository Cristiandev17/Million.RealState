using MediatR;
using Million.RealState.Domain.DTOs;

namespace Million.RealState.Application.Features.ImageProperty.Commands;

public record CreateImageOfPropertyCommand(List<PropertyImageDto> ImageDto): IRequest<ApiResponse<bool>>
{
}
