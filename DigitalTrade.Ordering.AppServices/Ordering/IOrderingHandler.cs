using DigitalTrade.Ordering.Api.Contracts.Ordering.Request;
using DigitalTrade.Ordering.Api.Contracts.Ordering.Response;

namespace DigitalTrade.Ordering.AppServices.Ordering;

public interface IOrderingHandler
{
    public Task DeleteOrderAsync(DeleteOrderRequest request, CancellationToken ct);

    public Task<GetOrdersByClientResponse> GetOrdersByClientAsync(GetOrdersByClientRequest request, CancellationToken ct);

    public Task<GetOrderByIdResponse> GetOrderByIdAsync(GetOrderByIdRequest request, CancellationToken ct);

    public Task SetOrderPrerequisites(SetOrderPrerequisitesRequest request, CancellationToken ct);
}