namespace DigitalTrade.Ordering.Api.Contracts;

public static class OrderingWebRoutes
{
    public const string BasePath = "ordering";

    public const string CreateOrder = "create";

    public const string UpdateOrder = "update";

    public const string DeleteOrder = $"delete/{Id}";

    public const string Id = "{id}";

    public const string GetOrders = "get";

    public const string GetOrdersByClient = $"{GetOrders}/by-client/{Id}";

    public const string GetOrdersByNane = $"{GetOrders}/by-name/{{orderName}}";
}