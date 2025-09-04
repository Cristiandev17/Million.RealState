using Azure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Million.RealState.Application.Features.Property.Commands;
using Million.RealState.Application.Features.Property.Queries;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Specifications;
using Million.RealState.Domain.Utilities;
using System.Net;

namespace Million.RealState.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PropertiesController(IMediator _mediator) : ControllerBase
    {
        // Creates a new property in the system
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] PropertyDto newProperty)
        {
            try
            {
                // Validate the incoming model data
                if (!ModelState.IsValid)
                {                  
                    return BadRequest(ApiResponse<bool>.ErrorResult(Constants.Invalid, Constants.ValidError));
                }                                                    

                var result = await _mediator.Send(new CreatePropertyCommand(newProperty));

                // Return bad request if creation failed due to business rules
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                // Return 201 Created with the result
                return CreatedAtAction(nameof(Post), result);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<string>.ErrorResult(ex.Message, Constants.ErrorCreatingProperty);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        // Updates the price of an existing property
        [HttpPatch]
        [Route("updatePrice/{propertyId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Patch([FromRoute] string propertyId, [FromBody] decimal price)
        {
            try
            {
                // Send command to update property price via MediatR
                var result = await _mediator.Send(new UpdatePriceOfPropertyCommand(propertyId, price));

                // Return bad request if update failed
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<string>.ErrorResult(ex.Message, Constants.ErrorUpdatingProperty);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        // Modifies an existing property with complete property data
        [HttpPut]
        [Route("modify")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Put([FromBody] PropertyDto property)
        {
            try
            {
                // Validate the incoming model data
                if (!ModelState.IsValid) 
                {
                    return BadRequest(ApiResponse<object>.ErrorResult(Constants.Invalid, Constants.ValidError));
                }

                // Send command to update property via MediatR
                var result = await _mediator.Send(new UpdatePropertyCommand(property));

                // Return bad request if update failed
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                // Return success response
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<string>.ErrorResult(ex.Message, Constants.ErrorUpdatingProperty);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        // Retrieves properties based on filter criteria
        [HttpGet()]
        [Route("propertiesWithFilters")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<List<PropertyDto>>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] PropertyFilterParams filterParams)
        {
            try
            {
                // Send query to get filtered properties via MediatR
                var result = await _mediator.Send(new GetPropertiesWithFiltersQuery(filterParams));

                // Return properties if found
                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                // Return not found if no properties match the criteria
                return NotFound(result);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<string>.ErrorResult(ex.Message, Constants.ErrorGettingProperties);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }           
        }
    }
}
