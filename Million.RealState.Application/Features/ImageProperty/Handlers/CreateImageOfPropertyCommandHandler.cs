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
    public async Task<ApiResponse<bool>> Handle(CreateImageOfPropertyCommand request, CancellationToken cancellationToken)
    {
        if (request.ImageDto == null)
        {
            return ApiResponse<bool>.ErrorResult(Constants.RequestNull, Constants.InvalidRequest);
        }

        if (!request.ImageDto.Any())
        {
            return ApiResponse<bool>.ErrorResult(Constants.ListImagesNoEmpty, Constants.ValidError);
        }

        var images = _mapper.Map<List<PropertyImageEntity>>(request.ImageDto);

        await _propertyImageRepository.CreatePropertyImageAsync(images);

        return ApiResponse<bool>.SuccessResult(true, Constants.SavedImages);
    }
}
