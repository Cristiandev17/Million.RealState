using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Million.RealState.Domain.Entities;

public class PropertyTraceEntity : BaseEntity
{
    public Guid PropertyId { get; set; }
        
    public DateOnly DateSale { get; set; }
           
    public string? BuyerName { get; set; }
       
    [EmailAddress]
    public string? BuyerEmail { get; set; }
       
    [Phone]
    public string? BuyerPhone { get; set; }
         
    public decimal Value { get; set; }
     
    public decimal Tax { get; set; }

    [ForeignKey("PropertyId")]
    public virtual PropertyEntity Property { get; set; }
}