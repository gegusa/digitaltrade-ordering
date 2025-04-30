namespace DigitalTrade.Ordering.Api.Contracts.Ordering.Request;

public class SetOrderPrerequisitesRequest
{
    public long OrderId { get; set; }

    public string DeliveryAddress { get; set; }

    #nullable enable
    public string? BankCardNumber { get; set; }
}