using AutoMapper;
using Million.RealState.Application.Features.Property.Commands;
using Million.RealState.Application.Features.Property.Handlers;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Entities;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Domain.Utilities;
using Moq;

namespace Million.RealState.Test.Handlers.Property;

public class UpdatePropertyCommandHandlerTest
{
    private Mock<IPropertyRepository> _mockPropertyRepository;
    private Mock<IMapper> _mockMapper;
    private UpdatePropertyCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockPropertyRepository = new Mock<IPropertyRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpdatePropertyCommandHandler(
            _mockPropertyRepository.Object,
            _mockMapper.Object);
    }

    [Test]
    public async Task HandleWhenPropertyIdIsEmptyReturnsErrorResult() 
    {
        // Arrange
        var propertyDto = new PropertyDto { Id = Guid.Empty };
        var command = new UpdatePropertyCommand(propertyDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.EqualTo(Constants.IdRequired));
        Assert.That(result.Message, Is.EqualTo(Constants.ValidError));
    }

    [Test]
    public async Task HandleWhenValidRequestReturnsSuccessResult() 
    {
        // Arrange
        var propertyId = Guid.NewGuid();
        var propertyDto = new PropertyDto { Id = propertyId };
        var propertyEntity = new PropertyEntity
        {
            Name = propertyDto.Name,
            Address = propertyDto.Address
        };

        var command = new UpdatePropertyCommand (propertyDto);

        _mockMapper.Setup(m => m.Map<PropertyEntity>(propertyDto))
                  .Returns(propertyEntity);

        _mockPropertyRepository.Setup(r => r.UpdateProperty(propertyEntity))
                             .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Data, Is.True);
        Assert.That(result.Message, Is.EqualTo(Constants.SavedProperty));
        Assert.That(result.Error, Is.Null);
    }

}
