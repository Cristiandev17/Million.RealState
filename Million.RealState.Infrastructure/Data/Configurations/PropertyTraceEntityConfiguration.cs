using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Million.RealState.Domain.Entities;

namespace Million.RealState.Infrastructure.Data.Configurations;

public class PropertyTraceEntityConfiguration : IEntityTypeConfiguration<PropertyTraceEntity>
{
    public void Configure(EntityTypeBuilder<PropertyTraceEntity> builder)
    {
        builder.ToTable("PropertyTrace");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.PropertyId)
            .IsRequired();
            
        builder.Property(x => x.DateSale)
            .IsRequired();
            
        builder.Property(x => x.BuyerName)
            .IsRequired()
            .HasMaxLength(150);
            
        builder.Property(x => x.BuyerEmail)
            .HasMaxLength(50);
        
        builder.Property(x => x.BuyerPhone)
            .HasMaxLength(50);
        
        builder.Property(x => x.Value)
            .IsRequired()
            .HasColumnType("decimal(10,2)");
            
        builder.Property(x => x.Tax)
            .IsRequired()
            .HasColumnType("decimal(10,2)");
            
        // Configure BaseEntity properties
        builder.Property(x => x.CreateDate)
            .IsRequired();            
   
        builder.Property(x => x.UpdateDate)
            .IsRequired();
      
        // indexing
        builder.HasIndex(e => e.PropertyId);
    }
}