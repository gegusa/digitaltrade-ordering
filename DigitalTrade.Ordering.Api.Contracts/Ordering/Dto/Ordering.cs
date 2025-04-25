namespace DigitalTrade.Ordering.Api.Contracts.Ordering.Dto;

public class Ordering
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public decimal Amount { get; set; }

    public OrderingStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}