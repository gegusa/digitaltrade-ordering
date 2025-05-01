using DigitalTrade.Ordering.Api.Contracts.Ordering.Dto;
using LinqToDB.Mapping;

namespace DigitalTrade.Ordering.Entities.Entities;

[Table(Name = "orders", Schema = "ordering")]
public class OrderEntity
{
    [PrimaryKey, Identity, Column("id")] public long Id { get; set; }

    [Column("customer_id"), NotNull] public long CustomerId { get; set; }

    [Column("payment"), NotNull] public string Payment { get; set; }

    #nullable enable
    [Column("credit_card")] public string? CreditCard { get; set; }

    [Column("shipping_address")] public string? ShippingAddress { get; set; }
    #nullable disable
    [Column("amount"), NotNull] public decimal Amount { get; set; }

    [Column("status"), NotNull] public OrderStatus Status { get; set; }

    [Column("created_at"), NotNull] public DateTime CreatedAt { get; set; }

    [Association(ThisKey = nameof(Id), OtherKey = nameof(OrderItemEntity.OrderId), CanBeNull = false)]
    public IReadOnlyList<OrderItemEntity> OrderItems { get; set; } = [];
}