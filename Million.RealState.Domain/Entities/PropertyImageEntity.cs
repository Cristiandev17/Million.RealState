using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Million.RealState.Domain.Entities;

public class PropertyImageEntity : BaseEntity
{ 
    public Guid PropertyId { get; set; }
        
    public string File { get; set; }

    public bool Enabled { get; set; }

    [ForeignKey("PropertyId")]
    public virtual PropertyEntity Property { get; set; }

}