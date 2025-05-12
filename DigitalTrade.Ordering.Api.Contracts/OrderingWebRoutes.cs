namespace DigitalTrade.Ordering.Api.Contracts;

public static class OrderingWebRoutes
{
    public const string BasePath = "ordering";

    public const string InitiatePayment = "payment";

    public const string DeleteOrder = "delete";

    public const string Id = "{id}";

    public const string GetOrders = "get";

    public const string GetOrdersById = $"{GetOrders}/{Id}";

    public const string GetOrdersByClient = $"{GetOrders}/by-client/{Id}";

    public const string SetOrderPrerequisites = "set-prerequisites";
}