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

    public async Task<Guid> CreatePropertyAsync(PropertyEntity newProperty)
    {
        await _context.Properties.AddAsync(newProperty);
        await _context.SaveChangesAsync();
        return newProperty.Id;
    }

    public async Task UpdatePriceOfProperty(Guid propertyId, decimal price)
    {
        await _context.Properties.Where(p => p.Id == propertyId).ExecuteUpdateAsync(set => set.SetProperty(c => c.Price, price));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProperty(PropertyEntity property) 
    {
        _context.Properties.Update(property);
        await _context.SaveChangesAsync();
    }

    public async Task<List<PropertyEntity>> GetFilteredPropertiesAsync(PropertyFilterParams filterParams)
    {
        var query = _context.Properties
            .Include(p => p.Owner)
            .Include(p => p.PropertyImages)
            .Include(p => p.PropertyTraces)
            .Where(p => p.Enabled)
            .AsQueryable();

        return await GetPropertiesApplyFilters(filterParams, ref query);
    }

    private Task<List<PropertyEntity>> GetPropertiesApplyFilters(PropertyFilterParams filterParams, ref IQueryable<PropertyEntity> query)
    {
        if (!string.IsNullOrEmpty(filterParams.OwnerId))
        {
            query = query.Where(p => p.OwnerId.ToString() == filterParams.OwnerId);
        }

        if (!string.IsNullOrEmpty(filterParams.CodeInternal))
        {
            query = query.Where(p => p.CodeInternal == filterParams.CodeInternal);
        }

        if (filterParams.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= filterParams.MinPrice.Value);
        }

        if (filterParams.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= filterParams.MaxPrice.Value);
        }

        if (filterParams.MinYear.HasValue)
        {
            query = query.Where(p => p.Year >= filterParams.MinYear.Value);
        }

        if (filterParams.MaxYear.HasValue)
        {
            query = query.Where(p => p.Year <= filterParams.MaxYear.Value);
        }

        return query.ToListAsync();
    }
}
