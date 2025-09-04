using System.ComponentModel.DataAnnotations;

namespace Million.RealState.Domain.Entities;

public class OwnerEntity : BaseEntity
{
   public string Name { get; set; }

   public long Identification { get; set; }

    public string? Address { get; set; }

    public string? Photo { get; set; }

    public DateOnly Birthday { get; set; }

    public bool Enabled { get; set; }

    public virtual ICollection<PropertyEntity> Properties { get; set; }
}