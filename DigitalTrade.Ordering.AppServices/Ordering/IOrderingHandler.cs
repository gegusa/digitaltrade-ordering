using DigitalTrade.Ordering.Api.Contracts.Ordering.Request;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Response;

namespace DigitalTrade.Ordering.AppServices.Ordering;

public interface IOrderingHandler
{
    public Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct);

    public Task<UpdateOrderResponse> UpdateOrderAsync(UpdateOrderRequest request, CancellationToken ct);

    public Task<DeleteOrderResponse> DeleteOrderAsync(DeleteOrderRequest request, CancellationToken ct);

    public Task<GetOrdersByClientResponse> GetOrdersByClientAsync(GetOrdersByClientRequest request, CancellationToken ct);

    public Task<GetOrdersResponse> GetOrdersAsync(GetOrdersRequest request, CancellationToken ct);

    public Task<GetOrdersByNameResponse> GetOrdersByNameAsync(GetOrdersByNameRequest request, CancellationToken ct);
}