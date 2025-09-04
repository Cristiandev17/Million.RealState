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

    // Creates multiple property images in the database in a single operation
    public async Task CreatePropertyImageAsync(List<PropertyImageEntity> newImage)
    {
        // Add multiple images to the context using AddRange for better performance
        await _context.PropertyImages.AddRangeAsync(newImage);

        // Persist changes to the database
        await _context.SaveChangesAsync();
    }
}
