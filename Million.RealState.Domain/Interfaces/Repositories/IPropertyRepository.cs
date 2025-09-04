using Million.RealState.Domain.Entities;
using Million.RealState.Domain.Specifications;

namespace Million.RealState.Domain.Interfaces.Repositories;

public interface IPropertyRepository
{
    Task<Guid> CreatePropertyAsync(PropertyEntity newProperty);

    Task UpdatePriceOfProperty(Guid propertyId, decimal price);

    Task UpdateProperty(PropertyEntity property);

    Task<List<PropertyEntity>> GetFilteredPropertiesAsync(PropertyFilterParams filterParams);
}
