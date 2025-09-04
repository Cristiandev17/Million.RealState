using MediatR;
using Microsoft.AspNetCore.Mvc;
using Million.RealState.Application.Features.Authentication.Commands;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Utilities;
using System.Net;

namespace Million.RealState.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationsController(IMediator _mediator) : ControllerBase
    {
        // Authenticates a user and returns a JWT token if credentials are valid
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] LoginDto request)
        {
            try
            {
                // Send authentication command to MediatR handler
                var response = await _mediator.Send(new AuthenticationCommand(request));

                // Return successful response with JWT token
                if (response.IsSuccess)
                {
                    return Ok(response);
                }

                // Return unauthorized if credentials are invalid
                return Unauthorized();
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<string>.ErrorResult(ex.Message, Constants.ErrorAuthenticating);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }           
        }
    }
}
