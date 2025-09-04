using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Million.RealState.Domain.Entities;

namespace Million.RealState.Infrastructure.Data.Configurations;

public class OwnerEntityConfiguration : IEntityTypeConfiguration<OwnerEntity>
{
    public void Configure(EntityTypeBuilder<OwnerEntity> builder)
    {
        builder.ToTable("Owner");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);
            
        builder.Property(x => x.Address)
            .HasMaxLength(250);

        builder.Property(x => x.Identification)
            .IsRequired();

        builder.Property(x => x.Photo)
            .HasMaxLength(500);

        builder.Property(x => x.Birthday);
            
        builder.Property(x => x.Enabled)
            .IsRequired()
            .HasDefaultValue(true);

        // BaseEntity properties
        builder.Property(x => x.CreateDate)
            .IsRequired();       
                   
        builder.Property(x => x.UpdateDate)
            .IsRequired();           
        
    }
}