using DigitalTrade.Ordering.Api.Contracts.Kafka;
using DigitalTrade.Ordering.Api.Contracts.Ordering;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Dto;
using DigitalTrade.Ordering.Entities;
using DigitalTrade.Ordering.Entities.Entities;
using KafkaFlow;
using KafkaFlow.Producers;
using LinqToDB;

namespace DigitalTrade.Ordering.AppServices.Kafka;

public class OrderCreatedHandler : IMessageHandler<OrderCreatedMessage>
{
    public const string ProducerName = nameof(OrderCreatedHandler);

    private readonly OrderingDataConnection _db;
    private readonly IProducerAccessor _producers;

    public OrderCreatedHandler(OrderingDataConnection db, IProducerAccessor producers)
    {
        _db = db;
        _producers = producers;
    }

    public async Task Handle(IMessageContext context, OrderCreatedMessage message)
    {
        var paymentEntity = new OrderEntity
        {
            OrderId = message.OrderId,
            Amount = message.Amount,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _db.InsertAsync(paymentEntity);

        // Симулируем оплату
        var success = OrderingHelper.GetRandomNumber(0, 2) == 1;

        paymentEntity.Status = success ? OrderStatus.Completed : OrderStatus.Failed;
    
        await _db.UpdateAsync(paymentEntity);

        await _producers[ProducerName].ProduceAsync(Topics.OrderingRequest, new
        {
            paymentEntity.OrderId,
            paymentEntity.Status
        });
    }
}