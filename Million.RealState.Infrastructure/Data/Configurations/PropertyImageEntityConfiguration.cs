using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Million.RealState.Domain.Entities;

namespace Million.RealState.Infrastructure.Data.Configurations;

public class PropertyImageEntityConfiguration : IEntityTypeConfiguration<PropertyImageEntity>
{
    public void Configure(EntityTypeBuilder<PropertyImageEntity> builder)
    {
        builder.ToTable("PropertyImage");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.PropertyId)
            .IsRequired();
            
        builder.Property(x => x.File)
            .IsRequired()
            .HasMaxLength(700);
            
        builder.Property(x => x.Enabled)
            .IsRequired()
            .HasDefaultValue(true);

        // Configure BaseEntity properties
        builder.Property(x => x.CreateDate)
            .IsRequired();
                   
        builder.Property(x => x.UpdateDate)
            .IsRequired();
              
        // indexing
        builder.HasIndex(e => e.PropertyId);
    }
}