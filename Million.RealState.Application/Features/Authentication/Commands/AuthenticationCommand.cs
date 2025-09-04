using MediatR;
using Million.RealState.Domain.DTOs;

namespace Million.RealState.Application.Features.Authentication.Commands;

public record AuthenticationCommand(LoginDto login) : IRequest<ApiResponse<string>>
{
}
