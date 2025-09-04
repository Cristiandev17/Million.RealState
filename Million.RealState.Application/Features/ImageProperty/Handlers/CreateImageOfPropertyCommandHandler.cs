using AutoMapper;
using MediatR;
using Million.RealState.Application.Features.ImageProperty.Commands;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Entities;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Domain.Utilities;
namespace Million.RealState.Application.Features.ImageProperty.Handlers;

public class CreateImageOfPropertyCommandHandler(
    IPropertyImageRepository _propertyImageRepository,
    IMapper _mapper) : IRequestHandler<CreateImageOfPropertyCommand, ApiResponse<bool>>
{
    // Handler for creating images associated with a property
    public async Task<ApiResponse<bool>> Handle(CreateImageOfPropertyCommand request, CancellationToken cancellationToken)
    {
        // Check if the image DTO list is null
        if (request.ImageDto == null)
        {
            return ApiResponse<bool>.ErrorResult(Constants.RequestNull, Constants.InvalidRequest);
        }

        // Check if the image DTO list is empty
        if (!request.ImageDto.Any())
        {
            return ApiResponse<bool>.ErrorResult(Constants.ListImagesNoEmpty, Constants.ValidError);
        }

        var images = _mapper.Map<List<PropertyImageEntity>>(request.ImageDto);

        // Save images to the repository
        await _propertyImageRepository.CreatePropertyImageAsync(images);

        return ApiResponse<bool>.SuccessResult(true, Constants.SavedImages);
    }
}
