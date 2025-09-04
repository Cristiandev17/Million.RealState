using AutoMapper;
using Million.RealState.Application.Features.ImageProperty.Commands;
using Million.RealState.Application.Features.ImageProperty.Handlers;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Entities;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Domain.Utilities;
using Moq;

namespace Million.RealState.Test.Handlers.PropertyImage;

public class CreateImageOfPropertyCommandHandlerTest
{
    private Mock<IPropertyImageRepository> _mockPropertyImageRepository;
    private Mock<IMapper> _mockMapper;
    private CreateImageOfPropertyCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockPropertyImageRepository = new Mock<IPropertyImageRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateImageOfPropertyCommandHandler(
            _mockPropertyImageRepository.Object,
            _mockMapper.Object);
    }

    [Test]
    public async Task HandleWhenImageDtoIsEmptyListReturnsErrorResult()
    {
        // Arrange
        var command = new CreateImageOfPropertyCommand(new List<PropertyImageDto>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.EqualTo(Constants.ListImagesNoEmpty));
        Assert.That(result.Message, Is.EqualTo(Constants.ValidError));
    }

    [Test]
    public async Task Handle_WhenMultipleImages_ProcessesAllImages()
    {
        // Arrange
        var imageDtos = new List<PropertyImageDto>
            {
                new PropertyImageDto { Id = Guid.NewGuid(), File = "https://example.com/image1.jpg" },
                new PropertyImageDto { Id = Guid.NewGuid(), File = "https://example.com/image2.jpg" },
                new PropertyImageDto { Id = Guid.NewGuid(), File = "https://example.com/image3.jpg" }
            };

        var command = new CreateImageOfPropertyCommand(imageDtos);

        var imageEntities = imageDtos.Select(dto => new PropertyImageEntity { File = dto.File }).ToList();

        _mockMapper.Setup(m => m.Map<List<PropertyImageEntity>>(imageDtos))
                  .Returns(imageEntities);

        _mockPropertyImageRepository.Setup(r => r.CreatePropertyImageAsync(imageEntities))
                                  .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        _mockPropertyImageRepository.Verify(r => r.CreatePropertyImageAsync(It.Is<List<PropertyImageEntity>>(list => list.Count == 3)), Times.Once);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var imageDtos = new List<PropertyImageDto>
        {
            new PropertyImageDto { Id = Guid.NewGuid(), File = "https://example.com/image1.jpg" }
        };

        var command = new CreateImageOfPropertyCommand(imageDtos);

        var imageEntities = new List<PropertyImageEntity>
        {
            new PropertyImageEntity {  File = imageDtos[0].File }
        };

        _mockMapper.Setup(m => m.Map<List<PropertyImageEntity>>(imageDtos))
                  .Returns(imageEntities);

        _mockPropertyImageRepository.Setup(r => r.CreatePropertyImageAsync(imageEntities))
                                  .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Data, Is.True);
        Assert.That(result.Message, Is.EqualTo(Constants.SavedImages));
        Assert.That(result.Error, Is.Null);
    }
}
