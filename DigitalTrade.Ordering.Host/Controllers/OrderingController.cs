using DigitalTrade.Ordering.Api.Contracts;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Request;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Response;
using DigitalTrade.Ordering.AppServices.Ordering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalTrade.Ordering.Host.Controllers;

[ApiController]
[Route(OrderingWebRoutes.BasePath)]
public class OrderingController : ControllerBase
{
    private readonly IOrderingHandler _handler;
    public OrderingController(IOrderingHandler handler)
    {
        _handler = handler;
    }

    [HttpPost(OrderingWebRoutes.DeleteOrder)]
    public Task DeleteOrder(
        [FromRoute] long id, CancellationToken ct)
    {
        return _handler.DeleteOrderAsync(new DeleteOrderRequest
        {
            OrderId = id
        }, ct);
    }

    [HttpGet(OrderingWebRoutes.GetOrders)]
    public Task<GetOrderByIdResponse> GetOrderById(
        [FromBody] GetOrderByIdRequest request, CancellationToken ct)
    {
        return _handler.GetOrderByIdAsync(request, ct);
    }

    [HttpGet(OrderingWebRoutes.GetOrdersByClient)]
    public Task<GetOrdersByClientResponse> GetOrdersByClient(
        [FromBody] long id, CancellationToken ct)
    {
        return _handler.GetOrdersByClientAsync(new GetOrdersByClientRequest
        {
            ClientId = id
        }, ct);
    }

    [HttpPost(OrderingWebRoutes.SetOrderPrerequisites)]
    public Task SetOrderPrerequisites(
        [FromBody] SetOrderPrerequisitesRequest request, CancellationToken ct)
    {
        return _handler.SetOrderPrerequisites(request, ct);
    }
}