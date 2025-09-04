using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Million.RealState.Application.Features.ImageProperty.Commands;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Utilities;
using System.Net;

namespace Million.RealState.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class PropertyImagesController(IMediator _mediator) : ControllerBase
{
    // Creates new images for a property
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Post([FromBody] List<PropertyImageDto> newImages)
    {
        try
        {
            // Validate the incoming model data
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResult(Constants.Invalid, Constants.ValidError));
            }

            // Send command to create property images via MediatR
            var result = await _mediator.Send(new CreateImageOfPropertyCommand(newImages));

            // Return bad request if image creation failed due to business rules
            if (!result.IsSuccess) 
            {
                return BadRequest(result);
            }

            // Return 201 Created with the result
            return CreatedAtAction(nameof(Post), result);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<string>.ErrorResult(ex.Message, Constants.ErrorCreatingImages);
            return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
        }
    }

}