using AutoMapper;
using Million.RealState.Application.Features.Property.Handlers;
using Million.RealState.Application.Features.Property.Queries;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Entities;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Domain.Specifications;
using Million.RealState.Domain.Utilities;
using Moq;

namespace Million.RealState.Test.Handlers.Property;

public class GetPropertiesWithFiltersQueryHandlerTest
{
    private Mock<IPropertyRepository> _mockPropertyRepository;
    private Mock<IMapper> _mockMapper;
    private GetPropertiesWithFiltersQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockPropertyRepository = new Mock<IPropertyRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetPropertiesWithFiltersQueryHandler(
            _mockPropertyRepository.Object,
            _mockMapper.Object);
    }

    [Test]
    public async Task HandleWhenRepositoryReturnsNullReturnsErrorResult()
    {
        // Arrange
        var query = new GetPropertiesWithFiltersQuery(new PropertyFilterParams());

        _mockPropertyRepository.Setup(r => r.GetFilteredPropertiesAsync(It.IsAny<PropertyFilterParams>()))
                             .ReturnsAsync((List<PropertyEntity>)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.EqualTo(Constants.NotFound));
        Assert.That(result.Message, Is.EqualTo(Constants.ErrorFilters));
        Assert.That(result.Data, Is.Null);
    }

    [Test]
    public async Task Handle_WhenRepositoryReturnsProperties_ReturnsSuccessWithMappedData()
    {
        // Arrange
        var query = new GetPropertiesWithFiltersQuery (new PropertyFilterParams());
        var properties = new List<PropertyEntity>
            {
                new PropertyEntity { Name = "Property 1", Price = 100000 },
                new PropertyEntity { Name = "Property 2", Price = 200000 }
            };

        _mockPropertyRepository.Setup(r => r.GetFilteredPropertiesAsync(It.IsAny<PropertyFilterParams>()))
                             .ReturnsAsync(properties);

        var expectedDtos = new List<PropertyDto>
            {
                new PropertyDto { Id = properties[0].Id, Name = "Property 1", Price = 100000 },
                new PropertyDto { Id = properties[1].Id, Name = "Property 2", Price = 200000 }
            };

        _mockMapper.Setup(m => m.Map<List<PropertyDto>>(properties))
                  .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Message, Is.EqualTo(Constants.Success));
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.Count, Is.EqualTo(2));
        Assert.That(result.Data[0].Name, Is.EqualTo("Property 1"));
        Assert.That(result.Data[1].Name, Is.EqualTo("Property 2"));
    }

    [Test]
    public async Task Handle_WhenCalledWithFilterParams_PassesParamsToRepository()
    {
        // Arrange
        var filterParams = new PropertyFilterParams
        {
            MinPrice = 100000,
            MaxPrice = 300000,           
        };

        var query = new GetPropertiesWithFiltersQuery(filterParams);

        var properties = new List<PropertyEntity>
            {
                new PropertyEntity { Name = "Property 1", Price = 200000 }
            };

        _mockPropertyRepository.Setup(r => r.GetFilteredPropertiesAsync(filterParams))
                             .ReturnsAsync(properties);

        var expectedDtos = new List<PropertyDto>
            {
                new PropertyDto { Id = properties[0].Id, Name = "Property 1", Price = 200000 }
            };

        _mockMapper.Setup(m => m.Map<List<PropertyDto>>(properties))
                  .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockPropertyRepository.Verify(r => r.GetFilteredPropertiesAsync(filterParams), Times.Once);
    }


}
