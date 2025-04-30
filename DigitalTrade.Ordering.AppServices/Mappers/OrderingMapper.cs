using DigitalTrade.Ordering.Api.Contracts.Ordering.Dto;
using DigitalTrade.Ordering.Entities.Entities;

namespace DigitalTrade.Ordering.AppServices.Mappers;

public static class OrderingMapper
{
    public static Order ToOrder(this OrderEntity orderEntity)
    {
        var order = new Order
        {
            Id = orderEntity.Id,
            Amount = orderEntity.Amount,
            CreatedAt = orderEntity.CreatedAt,
            CreditCard = orderEntity.CreditCard,
            Status = orderEntity.Status,
            Payment = orderEntity.Payment,
            ShippingAddress = orderEntity.ShippingAddress,
        };

        return order;
    }
}