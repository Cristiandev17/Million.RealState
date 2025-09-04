using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Million.RealState.Domain.Entities;

namespace Million.RealState.Infrastructure.Data;

public class RealStateDbContext : DbContext
{
    public RealStateDbContext(DbContextOptions<RealStateDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RealStateDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    public virtual DbSet<OwnerEntity> Owners { get; set; }

    public virtual DbSet<PropertyEntity> Properties { get; set; }

    public virtual DbSet<PropertyImageEntity> PropertyImages { get; set; }

    public virtual DbSet<PropertyTraceEntity> PropertyTraces { get; set; }
}

