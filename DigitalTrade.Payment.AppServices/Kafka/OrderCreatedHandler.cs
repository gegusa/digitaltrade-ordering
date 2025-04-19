using DigitalTrade.Payment.Api.Contracts.Kafka;
using DigitalTrade.Payment.Api.Contracts.Payment;
using DigitalTrade.Payment.Api.Contracts.Payment.Dto;
using DigitalTrade.Payment.Entities;
using DigitalTrade.Payment.Entities.Entities;
using KafkaFlow;
using KafkaFlow.Producers;
using LinqToDB;

namespace DigitalTrade.Payment.AppServices.Kafka;

public class OrderCreatedHandler : IMessageHandler<OrderCreatedMessage>
{
    public const string ProducerName = nameof(OrderCreatedHandler);

    private readonly PaymentDataConnection _db;
    private readonly IProducerAccessor _producers;

    public OrderCreatedHandler(PaymentDataConnection db, IProducerAccessor producers)
    {
        _db = db;
        _producers = producers;
    }

    public async Task Handle(IMessageContext context, OrderCreatedMessage message)
    {
        var paymentEntity = new PaymentEntity
        {
            OrderId = message.OrderId,
            Amount = message.Amount,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _db.InsertAsync(paymentEntity);

        // Симулируем оплату
        var success = PaymentHelper.GetRandomNumber(0, 2) == 1;

        paymentEntity.Status = success ? PaymentStatus.Completed : PaymentStatus.Failed;
    
        await _db.UpdateAsync(paymentEntity);

        await _producers[ProducerName].ProduceAsync(Topics.PaymentRequest, new
        {
            paymentEntity.OrderId,
            paymentEntity.Status
        });
    }
}