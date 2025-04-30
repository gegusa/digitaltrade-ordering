using System.Collections.Immutable;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Dto;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Request;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Response;
using DigitalTrade.Ordering.AppServices.Mappers;
using DigitalTrade.Ordering.Entities;
using KafkaFlow;
using LinqToDB;

namespace DigitalTrade.Ordering.AppServices.Ordering;

internal class OrderingHandler : IOrderingHandler
{
    private readonly OrderingDataConnection _db;
    private readonly IMessageProducer _messageProducer;

    public OrderingHandler(OrderingDataConnection db, IMessageProducer messageProducer)
    {
        _db = db;
        _messageProducer = messageProducer;
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
}