namespace Million.RealState.Domain.Specifications;

public class PropertyFilterParams
{
    public string? OwnerId { get; set; }

    public string? CodeInternal { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public int? MinYear { get; set; }

    public int? MaxYear { get; set; }
}
