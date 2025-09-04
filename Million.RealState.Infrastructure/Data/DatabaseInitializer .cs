using Million.RealState.Domain.Entities;

namespace Million.RealState.Infrastructure.Data;

public class DatabaseInitializer
{
    // Initializes the database with seed data if tables are empty
    public static void Initialize(RealStateDbContext context)
    {
        // Ensure the database is created (if using Code First approach)
        context.Database.EnsureCreated();

        // Seed Owners table if empty
        if (!context.Owners.Any())
        {
            var owners = new List<OwnerEntity>()
            {
                new()
                {
                    Name = "John Doe",
                    Identification = 1234567890,
                    Address = "123 Main St",
                    Photo = null,
                    Birthday = new DateOnly(1980, 5, 15),
                    Enabled = true,                    
                    CreateDate = DateTime.UtcNow,
                },
                new()
                {
                    Name = "Jane Smith",
                    Identification = 9876543210,
                    Address = "456 Elm St",
                    Photo = null,
                    Birthday = new DateOnly(1990, 8, 22),
                    Enabled = true,
                    CreateDate = DateTime.UtcNow,
                },
            };

            context.Owners.AddRange(owners);
            context.SaveChanges();
        }

        // Seed Properties table if empty
        if (!context.Properties.Any())
        {
            // Get the first owner to associate properties with
            var owner = context.Owners.First();
            var properties = new List<PropertyEntity>
            {
                new()
                {
                    Name = "Luxury Villa",
                    Address = "789 Ocean Drive",
                    Price = 1200000m,
                    CodeInternal = "LV123",
                    Year = 2020,
                    OwnerId = owner.Id,
                    Enabled = true,
                    CreateDate = DateTime.UtcNow,
                },
                new()
                {
                    Name = "Downtown Apartment",
                    Address = "101 City Center",
                    Price = 350000m,
                    CodeInternal = "DA456",
                    Year = 2018,
                    OwnerId = owner.Id,
                    Enabled = true,
                    CreateDate = DateTime.UtcNow,                    
                }
            };

            context.Properties.AddRange(properties);
            context.SaveChanges();
        }

        // Seed PropertyTraces table if empty
        if (!context.PropertyTraces.Any())
        {
            // Get the first property to associate traces with
            var property = context.Properties.First();
            var traces = new List<PropertyTraceEntity>
            {
                new()
                {
                    PropertyId = property.Id,
                    DateSale = new DateOnly(2023, 3, 10),
                    BuyerName = "Alice Johnson",
                    BuyerEmail = "alice.johnson@example.com",
                    BuyerPhone = "555-1234",
                    Value = 1150000m,
                    Tax = 150000m,
                    CreateDate = DateTime.UtcNow,
                },
                new()
                {
                    PropertyId = property.Id,
                    DateSale = new DateOnly(2024, 1, 5),
                    BuyerName = "Bob Lee",
                    BuyerEmail = "bob.lee@example.com",
                    BuyerPhone = "555-5678",
                    Value = 1200000m,
                    Tax = 160000m,                    
                    UpdateDate = DateTime.UtcNow,
                }
            };

            context.PropertyTraces.AddRange(traces);
            context.SaveChanges();
        }
    }
}
