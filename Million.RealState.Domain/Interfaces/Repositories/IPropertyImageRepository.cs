using Million.RealState.Domain.Entities;

namespace Million.RealState.Domain.Interfaces.Repositories;

public interface IPropertyImageRepository
{
    Task CreatePropertyImageAsync(List<PropertyImageEntity> newImage);
}
