namespace DigitalTrade.Ordering.Api.Contracts.Ordering.Dto;

public class Order
{
    public long Id { get; set; }

    public decimal Amount { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Payment { get; set; }

    #nullable enable
    public string? ShippingAddress { get; set; }

    public string? CreditCard { get; set; }
}