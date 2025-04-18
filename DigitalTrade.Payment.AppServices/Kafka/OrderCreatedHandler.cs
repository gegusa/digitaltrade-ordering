using DigitalTrade.Payment.Api.Contracts.Kafka;
using DigitalTrade.Payment.Api.Contracts.Payment;
using DigitalTrade.Payment.Api.Contracts.Payment.Dto;
using DigitalTrade.Payment.Entities;
using DigitalTrade.Payment.Entities.Entities;
using KafkaFlow;
using LinqToDB;

namespace DigitalTrade.Payment.AppServices.Kafka;

public class OrderCreatedHandler : IMessageHandler<OrderCreatedMessage>
{
    private readonly PaymentDataConnection _db;
    private readonly IMessageProducer _producer;

    public OrderCreatedHandler(PaymentDataConnection db, IMessageProducer producer)
    {
        _db = db;
        _producer = producer;
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

        await _producer.ProduceAsync(Topics.PaymentRequest, new
        {
            paymentEntity.OrderId,
            paymentEntity.Status
        });
    }
}