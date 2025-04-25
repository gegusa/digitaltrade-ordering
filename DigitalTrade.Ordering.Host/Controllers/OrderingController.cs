using DigitalTrade.Ordering.Api.Contracts;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Request;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Response;
using DigitalTrade.Ordering.AppServices.Ordering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalTrade.Ordering.Host.Controllers;

[Authorize]
[ApiController]
[Route(OrderingWebRoutes.BasePath)]
public class OrderingController : ControllerBase
{
    private readonly IOrderingHandler _handler;
    public OrderingController(IOrderingHandler handler)
    {
        _handler = handler;
    }

    [AllowAnonymous]
    [HttpPost(OrderingWebRoutes.CreateOrder)]
    public Task<CreateOrderResponse> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        return _handler.CreateOrderAsync(request, ct);
    }

    [AllowAnonymous]
    [HttpPost(OrderingWebRoutes.UpdateOrder)]
    public Task<UpdateOrderResponse> UpdateOrder([FromBody] UpdateOrderRequest request, CancellationToken ct)
    {
        return _handler.UpdateOrderAsync(request, ct);
    }
    
    [AllowAnonymous]
    [HttpPost(OrderingWebRoutes.DeleteOrder)]
    public Task<DeleteOrderResponse> DeleteOrder([FromRoute] long id, CancellationToken ct)
    {
        return _handler.DeleteOrderAsync(new DeleteOrderRequest
        {
            OrderId = id
        }, ct);
    }

    [AllowAnonymous]
    [HttpGet(OrderingWebRoutes.GetOrders)]
    public Task<GetOrdersResponse> GetOrders([FromBody] GetOrdersRequest request, CancellationToken ct)
    {
        return _handler.GetOrdersAsync(request, ct);
    }
    
    [AllowAnonymous]
    [HttpGet(OrderingWebRoutes.GetOrdersByNane)]
    public Task<GetOrdersByNameResponse> GetOrdersByName([FromBody] GetOrdersByNameRequest request, CancellationToken ct)
    {
        return _handler.GetOrdersByNameAsync(request, ct);
    }

    [AllowAnonymous]
    [HttpGet(OrderingWebRoutes.GetOrdersByClient)]
    public Task<GetOrdersByClientResponse> GetOrdersByClient([FromBody] long id, CancellationToken ct)
    {
        return _handler.GetOrdersByClientAsync(new GetOrdersByClientRequest
        {
            ClientId = id
        }, ct);
    }
}