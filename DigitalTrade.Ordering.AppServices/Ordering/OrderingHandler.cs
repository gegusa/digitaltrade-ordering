using System.Collections.Immutable;
using System.Transactions;
using DigitalTrade.Ordering.Api.Contracts.Kafka;
using DigitalTrade.Ordering.Api.Contracts.Kafka.Events;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Dto;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Request;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Response;
using DigitalTrade.Ordering.AppServices.Mappers;
using DigitalTrade.Ordering.Entities;
using KafkaFlow;
using KafkaFlow.Producers;
using LinqToDB;

namespace DigitalTrade.Ordering.AppServices.Ordering;

internal class OrderingHandler : IOrderingHandler
{
    private readonly OrderingDataConnection _db;
    private readonly IProducerAccessor _producers;

    public OrderingHandler(OrderingDataConnection db, IProducerAccessor producers)
    {
        _db = db;
        _producers = producers;
    }

    public async Task DeleteOrderAsync(DeleteOrderRequest request, CancellationToken ct)
    {
        var numOfDeletedOrders = await _db.Orders
            .Where(o => o.Id == request.OrderId)
            .DeleteAsync(ct);

        if (numOfDeletedOrders == 0)
        {
            throw new InvalidOperationException("Order not found");
        }
    }

    public async Task<GetOrdersByClientResponse> GetOrdersByClientAsync(GetOrdersByClientRequest request, CancellationToken ct)
    {
        var entities = await _db.Orders
            .Where(o => o.CustomerId == request.ClientId)
            .ToArrayAsync(ct);

        return new GetOrdersByClientResponse
        {
            Orders = entities.Select(OrderingMapper.ToOrder).ToArray()
        };
    }

    public async Task<GetOrderByIdResponse> GetOrderByIdAsync(GetOrderByIdRequest request, CancellationToken ct)
    {
        var orderEntity = await _db.Orders
            .Where(o => o.Id == request.OrderId)
            .SingleOrDefaultAsync(ct);

        if (orderEntity is null)
        {
            throw new InvalidOperationException($"Order with id = {request.OrderId} not found");
        }

        return new GetOrderByIdResponse
        {
            Order = orderEntity.ToOrder()
        };
    }

    public async Task SetOrderPrerequisites(SetOrderPrerequisitesRequest request, CancellationToken ct)
    {
        var orderEntity = await _db.Orders
            .Where(o => o.Id == request.OrderId)
            .SingleAsync(ct);

        if (orderEntity.Status is not OrderStatus.Pending)
        {
            throw new InvalidOperationException($"Order {request.OrderId} is not pending");
        }

        orderEntity.ShippingAddress = request.DeliveryAddress;
        if (orderEntity.Payment is "card")
            orderEntity.CreditCard = request.BankCardNumber;

        await _db.UpdateAsync(orderEntity, token: ct);
    }

    public async Task InitiatePayment(InitiatePaymentRequest request, CancellationToken ct)
    {
        var orderEntity = await _db.Orders
            .Where(o => o.Id == request.OrderId)
            .SingleOrDefaultAsync(ct);

        if (orderEntity is null)
            throw new InvalidOperationException($"Order with id={request.OrderId} does not exist");

        if (orderEntity.Status is not OrderStatus.Created)
            throw new InvalidOperationException($"Order with id={request.OrderId} is not pending");

        if (orderEntity.Payment is not "card")
            throw new InvalidOperationException($"Payment method of order with id={request.OrderId} is not card");

        if (orderEntity.ShippingAddress is null)
            throw new InvalidOperationException($"No shipping address specified in order with id={request.OrderId}");

        if (orderEntity.CreditCard is null)
            throw new InvalidOperationException($"Credit card info of order with id={request.OrderId} is empty");

        using (var scope = new TransactionScope())
        {
            orderEntity.Status = OrderStatus.Pending;
            await _db.UpdateAsync(orderEntity, token: ct);

            var message = new PaymentRequestedEvent
            {
                OrderId = orderEntity.Id,
                CreditCard = orderEntity.CreditCard,
            };

            await _producers[Topics.PaymentRequestedTopicProducerName].ProduceAsync(
                Topics.PaymentRequestedTopicName,
                message.OrderId.ToString(),
                message);
        }
    }
}