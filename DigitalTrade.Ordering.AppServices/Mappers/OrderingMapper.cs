using DigitalTrade.Basket.Api.Contracts.Dto;
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

    public static OrderItemEntity ToOrderItemEntity(this ShoppingCartItem shoppingCartItem, long orderId)
    {
        return new OrderItemEntity
        {
            Quantity = shoppingCartItem.Quantity,
            ProductId = shoppingCartItem.ProductId,
            PricePerItem = shoppingCartItem.PriceAtAdding,
            Name = shoppingCartItem.Name,
            OrderId = orderId
        };
    }
}