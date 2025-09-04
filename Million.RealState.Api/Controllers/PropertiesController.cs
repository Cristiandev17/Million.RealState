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
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] PropertyDto newProperty)
        {
            try
            {
                if (!ModelState.IsValid)
                {                  
                    return BadRequest(ApiResponse<bool>.ErrorResult(Constants.Invalid, Constants.ValidError));
                }                                                    

                var result = await _mediator.Send(new CreatePropertyCommand(newProperty));

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return CreatedAtAction(nameof(Post), result);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<string>.ErrorResult(ex.Message, Constants.ErrorCreatingProperty);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPatch]
        [Route("updatePrice/{propertyId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Patch([FromRoute] string propertyId, [FromBody] decimal price)
        {
            try
            {
                var result = await _mediator.Send(new UpdatePriceOfPropertyCommand(propertyId, price));

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

        [HttpPut]
        [Route("modify")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Put([FromBody] PropertyDto property)
        {
            try
            {
                if (!ModelState.IsValid) 
                {
                    return BadRequest(ApiResponse<object>.ErrorResult(Constants.Invalid, Constants.ValidError));
                }                        

                var result = await _mediator.Send(new UpdatePropertyCommand(property));

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

        [HttpGet()]
        [Route("propertiesWithFilters")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<List<PropertyDto>>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] PropertyFilterParams filterParams)
        {
            try
            {
                var result = await _mediator.Send(new GetPropertiesWithFiltersQuery(filterParams));

                if (result.IsSuccess)
                {
                    return Ok(result);
                }

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
