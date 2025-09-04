using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Million.RealState.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; protected set; }

    public DateTime CreateDate { get; set; }  

    public DateTime UpdateDate { get; set; } = DateTime.UtcNow;
}
