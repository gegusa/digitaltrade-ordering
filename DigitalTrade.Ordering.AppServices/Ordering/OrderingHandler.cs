using DigitalTrade.Ordering.Api.Contracts.Ordering.Request;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Response;

namespace DigitalTrade.Ordering.AppServices.Ordering;

internal class OrderingHandler : IOrderingHandler
{
    public Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<UpdateOrderResponse> UpdateOrderAsync(UpdateOrderRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<DeleteOrderResponse> DeleteOrderAsync(DeleteOrderRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<GetOrdersByClientResponse> GetOrdersByClientAsync(GetOrdersByClientRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<GetOrdersResponse> GetOrdersAsync(GetOrdersRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<GetOrdersByNameResponse> GetOrdersByNameAsync(GetOrdersByNameRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}