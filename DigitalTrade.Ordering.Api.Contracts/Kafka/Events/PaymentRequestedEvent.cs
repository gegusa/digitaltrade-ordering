namespace DigitalTrade.Ordering.Api.Contracts.Kafka.Events;

public class PaymentRequestedEvent
{
    public long OrderId { get; set; }

    public string CreditCard { get; set; }
}