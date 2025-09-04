using AutoMapper;
using Million.RealState.Application.Features.Property.Commands;
using Million.RealState.Application.Features.Property.Handlers;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Entities;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Domain.Utilities;
using Moq;

namespace Million.RealState.Test.Handlers.Property;

public class CreatePropertyCommandHandlerTest
{
    private Mock<IPropertyRepository> _propertyRepositoryMock;   
    private Mock<IMapper> _mapperMock;
    private CreatePropertyCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _propertyRepositoryMock = new Mock<IPropertyRepository>();      
        _mapperMock = new Mock<IMapper>();
        _handler = new CreatePropertyCommandHandler(
            _propertyRepositoryMock.Object,         
            _mapperMock.Object);
    }

    [Test]
    public async Task HandleWithValidPropertyShouldCreatePropertyAndReturnSuccess()
    {
        // Arrange
        var propertyId = Guid.NewGuid();
        var propertyDto = new PropertyDto
        {
            Name = "Test Property",
            Address = "123 Test St",
            Image = "base64-image-string"
        };

        var propertyEntity = new PropertyEntity
        {           
            Name = propertyDto.Name,
            Address = propertyDto.Address
        };

        var command = new CreatePropertyCommand(propertyDto);

        _mapperMock.Setup(m => m.Map<PropertyEntity>(propertyDto))
                  .Returns(propertyEntity);

        _propertyRepositoryMock.Setup(r => r.CreatePropertyAsync(propertyEntity))
                              .ReturnsAsync(propertyId);       

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Data, Is.True);
        Assert.That(result.Message, Is.Not.Null.And.EqualTo(Constants.SavedProperty));

        _mapperMock.Verify(m => m.Map<PropertyEntity>(propertyDto), Times.Once);
        _propertyRepositoryMock.Verify(r => r.CreatePropertyAsync(propertyEntity), Times.Once);        
    }   

    [Test]
    public void Handle_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var propertyDto = new PropertyDto { Name = "Test Property" };
        var command = new CreatePropertyCommand(propertyDto);
        var propertyEntity = new PropertyEntity { Name = propertyDto.Name };

        _mapperMock.Setup(m => m.Map<PropertyEntity>(propertyDto))
                  .Returns(propertyEntity);

        _propertyRepositoryMock.Setup(r => r.CreatePropertyAsync(propertyEntity))
                             .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}
