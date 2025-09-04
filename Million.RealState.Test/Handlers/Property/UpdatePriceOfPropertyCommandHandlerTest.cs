using Million.RealState.Application.Features.Property.Commands;
using Million.RealState.Application.Features.Property.Handlers;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Domain.Utilities;
using Moq;

namespace Million.RealState.Test.Handlers.Property;

public class UpdatePriceOfPropertyCommandHandlerTest
{
    private Mock<IPropertyRepository> _mockPropertyRepository;
    private UpdatePriceOfPropertyCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockPropertyRepository = new Mock<IPropertyRepository>();
        _handler = new UpdatePriceOfPropertyCommandHandler(_mockPropertyRepository.Object);
    }

    [Test]
    public async Task HandleWhenPriceIsZeroReturnsErrorResult()
    {
        // Arrange
        var command = new UpdatePriceOfPropertyCommand(Guid.NewGuid().ToString(), 0 );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess , Is.False);
        Assert.That(result.Error, Is.EqualTo(Constants.PriceNoZero));
        Assert.That(result.Message, Is.EqualTo(Constants.ValidError));
    }

    [Test]
    public async Task Handle_WhenPriceIsNegative_ReturnsErrorResult()
    {
        // Arrange
        var command = new UpdatePriceOfPropertyCommand (Guid.NewGuid().ToString(), -100);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.EqualTo(Constants.PriceNoZero));
        Assert.That(result.Message, Is.EqualTo(Constants.ValidError));
    }

    [Test]
    public async Task Handle_WhenValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var propertyId = Guid.NewGuid();
        var price = 300000m;
        var command = new UpdatePriceOfPropertyCommand(propertyId.ToString(), price);

        _mockPropertyRepository.Setup(r => r.UpdatePriceOfProperty(It.IsAny<Guid>(), It.IsAny<decimal>()))
                             .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Data, Is.True);
        Assert.That(result.Message, Is.EqualTo(Constants.UpdatedPrice));
        Assert.That(result.Error, Is.Null);
    }
}
