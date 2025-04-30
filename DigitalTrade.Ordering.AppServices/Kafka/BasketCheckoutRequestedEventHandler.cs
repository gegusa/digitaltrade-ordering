using DigitalTrade.Basket.Api.Contracts.Kafka.Events;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Dto;
using DigitalTrade.Ordering.Entities;
using DigitalTrade.Ordering.Entities.Entities;
using KafkaFlow;
using KafkaFlow.Producers;
using LinqToDB;

namespace DigitalTrade.Ordering.AppServices.Kafka;

public class BasketCheckoutRequestedEventHandler : IMessageHandler<BasketCheckoutRequestedEvent>
{
    private readonly OrderingDataConnection _db;
    private readonly IProducerAccessor _producers;

    public BasketCheckoutRequestedEventHandler(OrderingDataConnection db, IProducerAccessor producers)
    {
        _db = db;
        _producers = producers;
    }

    public async Task Handle(IMessageContext context, BasketCheckoutRequestedEvent message)
    {
        // здесь должен быть синхронный запрос в clients для получения информации о заказе
        var OrderEntity = new OrderEntity
        {
            Amount = message.TotalPrice,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            CustomerId = message.ClientId
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