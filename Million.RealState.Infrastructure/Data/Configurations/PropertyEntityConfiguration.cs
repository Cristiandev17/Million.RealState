using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Million.RealState.Domain.Entities;

namespace Million.RealState.Infrastructure.Data.Configurations;

public class PropertyEntityConfiguration : IEntityTypeConfiguration<PropertyEntity>
{
    public void Configure(EntityTypeBuilder<PropertyEntity> builder)
    {
        builder.ToTable("Property");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(e => e.Address)
            .IsRequired()
            .HasMaxLength(250);
            
        builder.Property(e => e.Price)
            .IsRequired()
            .HasColumnType("decimal(16,2)");
        
        builder.Property(e => e.CodeInternal)
            .HasMaxLength(8);
        
        builder.Property(e => e.Year);
        
        builder.Property(x => x.Enabled)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(e => e.OwnerId)
            .IsRequired();
        
        // BaseEntity properties
        builder.Property(x => x.CreateDate)
            .IsRequired();      
            
        builder.Property(x => x.UpdateDate)
            .IsRequired();
            
        // indexing
        builder.HasIndex(e => e.OwnerId);
    }
}
