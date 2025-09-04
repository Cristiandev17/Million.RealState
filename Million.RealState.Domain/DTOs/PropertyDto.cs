namespace Million.RealState.Domain.DTOs;

public record PropertyDto
{
    public Guid? Id { get; init; }

    public Guid OwnerId { get; set; }

    public string Name { get; init; }

    public string Address { get; init; }

    public decimal Price { get; init; }

    public string? CodeInternal { get; init; }

    public int Year { get; init; }

    public string? Image { get; set; }

    public bool? Enabled { get; init; }   
}
