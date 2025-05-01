using DigitalTrade.Ordering.Api.Contracts.Kafka.Events;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Dto;
using DigitalTrade.Ordering.Entities;
using KafkaFlow;
using LinqToDB;

namespace DigitalTrade.Ordering.AppServices.Kafka;

public class PaymentRequestedEventHandler : IMessageHandler<PaymentRequestedEvent>
{
    private readonly OrderingDataConnection _db;

    public PaymentRequestedEventHandler(OrderingDataConnection db)
    {
        _db = db;
    }

    public async Task Handle(IMessageContext context, PaymentRequestedEvent message)
    {
        var order = await _db.Orders
            .Where(o => o.Id == message.OrderId)
            .SingleOrDefaultAsync();

        if (order is null)
            throw new InvalidOperationException($"Order with id {message.OrderId} not found");

        var status = OrderingHelper.GetRandomNumber(0, 2) % 2 == 0
            ? OrderStatus.Completed
            : OrderStatus.Failed;

        order.Status = status;

        await _db.UpdateAsync(order);
    }
}