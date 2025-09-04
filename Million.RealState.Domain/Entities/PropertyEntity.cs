using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Million.RealState.Domain.Entities;

public class PropertyEntity : BaseEntity
{  
    public string Name { get; set; }

    public string Address { get; set; }

    public decimal Price { get; set; }
        
    public string? CodeInternal { get; set; }
        
    public int Year { get; set; }

    public Guid OwnerId { get; set; }

    public bool Enabled { get; set; }

    [ForeignKey("OwnerId")]
    public virtual OwnerEntity Owner { get; set; }

    public virtual ICollection<PropertyImageEntity> PropertyImages { get; set; }

    public virtual ICollection<PropertyTraceEntity> PropertyTraces { get; set; }
}