namespace Million.RealState.Domain.DTOs;

public record PropertyTraceDto
{
    public Guid Id { get; init; }

    public DateOnly DateSale { get; init; }

    public string? BuyerName { get; init; }

    public string? BuyerEmail { get; init; }

    public string? BuyerPhone { get; init; }

    public decimal Value { get; init; }

    public decimal Tax { get; init; }
}
