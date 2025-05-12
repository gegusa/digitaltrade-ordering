using DigitalTrade.Basket.Api.Contracts.Kafka.Events;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Dto;
using DigitalTrade.Ordering.AppServices.Mappers;
using DigitalTrade.Ordering.Entities;
using DigitalTrade.Ordering.Entities.Entities;
using KafkaFlow;
using KafkaFlow.Producers;
using LinqToDB;
using LinqToDB.Data;

namespace DigitalTrade.Ordering.AppServices.Kafka;

public class BasketCheckoutRequestedEventHandler : IMessageHandler<BasketCheckoutRequestedEvent>
{
    private readonly OrderingDataConnection _db;

    public BasketCheckoutRequestedEventHandler(OrderingDataConnection db)
    {
        _db = db;
    }

    public async Task Handle(IMessageContext context, BasketCheckoutRequestedEvent message)
    {
        var basket = message.Basket;

        var orderEntity = new OrderEntity
        {
            Amount = message.TotalPrice,
            Status = OrderStatus.Created,
            CreatedAt = DateTime.UtcNow,
            Payment = "card",
            CustomerId = message.ClientId
        };

        var orderId = await _db.InsertWithInt64IdentityAsync(orderEntity);

        var result = await _db.BulkCopyAsync(new BulkCopyOptions
        {
            BulkCopyType = BulkCopyType.ProviderSpecific
        }, basket.Select(b => b.ToOrderItemEntity(orderId)));
    }
}