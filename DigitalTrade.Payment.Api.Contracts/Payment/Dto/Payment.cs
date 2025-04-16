namespace DigitalTrade.Payment.Api.Contracts.Payment.Dto;

public class Payment
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public decimal Amount { get; set; }

    public PaymentStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}