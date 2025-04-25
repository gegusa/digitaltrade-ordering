using DigitalTrade.Ordering.Api.Contracts.Ordering.Dto;
using LinqToDB.Mapping;

namespace DigitalTrade.Ordering.Entities.Entities;

[Table(Name = "payments", Schema = "payments")]
public class OrderingEntity
{
    [PrimaryKey, Identity, Column("id")] public long Id { get; set; }

    [Column("order_id"), NotNull] public long OrderId { get; set; }

    [Column("amount"), NotNull] public decimal Amount { get; set; }

    [Column("status"), NotNull] public OrderingStatus Status { get; set; }

    [Column("created_at"), NotNull] public DateTime CreatedAt { get; set; }
}