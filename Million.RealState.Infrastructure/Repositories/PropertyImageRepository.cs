using Million.RealState.Domain.Entities;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Infrastructure.Data;

namespace Million.RealState.Infrastructure.Repositories;

public class PropertyImageRepository : IPropertyImageRepository
{
    private readonly RealStateDbContext _context;

    public PropertyImageRepository(RealStateDbContext context)
    {
        _context = context;
    }

    public async Task CreatePropertyImageAsync(List<PropertyImageEntity> newImage)
    {
        await _context.PropertyImages.AddRangeAsync(newImage);
        await _context.SaveChangesAsync();
    }
}
