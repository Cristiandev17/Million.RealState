namespace Million.RealState.Domain.DTOs;

public record PropertyImageDto
{
    public Guid? Id { get; set; }

    public Guid PropertyId { get; set; }

    public string File { get; set; }    
}
