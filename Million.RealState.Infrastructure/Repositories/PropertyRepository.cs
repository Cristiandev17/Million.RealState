using Microsoft.EntityFrameworkCore;
using Million.RealState.Domain.Entities;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Domain.Specifications;
using Million.RealState.Infrastructure.Data;

namespace Million.RealState.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly RealStateDbContext _context;

    public PropertyRepository(RealStateDbContext context)
    {
        _context = context;
    }

    // Creates a new property in the database
    public async Task<Guid> CreatePropertyAsync(PropertyEntity newProperty)
    {
        await _context.Properties.AddAsync(newProperty);
        await _context.SaveChangesAsync();
        return newProperty.Id;
    }

    // Updates the price of a specific property
    public async Task UpdatePriceOfProperty(Guid propertyId, decimal price)
    {
        // Uses ExecuteUpdate for efficient bulk update without loading the entity
        await _context.Properties.Where(p => p.Id == propertyId).ExecuteUpdateAsync(set => set.SetProperty(c => c.Price, price));
        await _context.SaveChangesAsync();
    }

    // Updates an existing property entity in the database
    public async Task UpdateProperty(PropertyEntity property) 
    {
        _context.Properties.Update(property);
        await _context.SaveChangesAsync();
    }

    // Retrieves a list of properties filtered by specified criteria
    public async Task<List<PropertyEntity>> GetFilteredPropertiesAsync(PropertyFilterParams filterParams)
    {
        // Build the base query and enabled filter
        var query = _context.Properties           
            .Where(p => p.Enabled)
            .AsQueryable();

        return await GetPropertiesApplyFilters(filterParams, ref query);
    }

    // Applies filtering conditions to the property query based on filter parameters
    private Task<List<PropertyEntity>> GetPropertiesApplyFilters(PropertyFilterParams filterParams, ref IQueryable<PropertyEntity> query)
    {
        // Filter by owner ID if provided
        if (!string.IsNullOrEmpty(filterParams.OwnerId))
        {
            query = query.Where(p => p.OwnerId.ToString() == filterParams.OwnerId);
        }

        // Filter by internal code if provided
        if (!string.IsNullOrEmpty(filterParams.CodeInternal))
        {
            query = query.Where(p => p.CodeInternal == filterParams.CodeInternal);
        }

        // Filter by minimum price if provided
        if (filterParams.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= filterParams.MinPrice.Value);
        }

        // Filter by maximum price if provided
        if (filterParams.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= filterParams.MaxPrice.Value);
        }

        // Filter by minimum year if provided
        if (filterParams.MinYear.HasValue)
        {
            query = query.Where(p => p.Year >= filterParams.MinYear.Value);
        }

        // Filter by maximum year if provided
        if (filterParams.MaxYear.HasValue)
        {
            query = query.Where(p => p.Year <= filterParams.MaxYear.Value);
        }

        // Execute the query and return results
        return query.ToListAsync();
    }
}
