namespace DigitalTrade.Ordering.Api.Contracts.Kafka;

public class OrderCreatedMessage
{
    public long OrderId { get; set; }
    public decimal Amount { get; set; }
}