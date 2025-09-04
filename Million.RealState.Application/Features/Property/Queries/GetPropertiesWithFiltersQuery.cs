using MediatR;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Specifications;

namespace Million.RealState.Application.Features.Property.Queries;

public record GetPropertiesWithFiltersQuery(PropertyFilterParams Params) : IRequest<ApiResponse<List<PropertyDto>>>
{
}
