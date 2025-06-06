﻿using DigitalTrade.Ordering.Api.Contracts.Ordering.Dto;

namespace DigitalTrade.Ordering.Api.Contracts.Ordering.Response;

public class GetOrdersByClientResponse
{
    public IReadOnlyCollection<Order> Orders { get; set; }
}