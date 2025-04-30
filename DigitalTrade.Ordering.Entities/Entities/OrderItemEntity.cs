using LinqToDB.Mapping;

namespace DigitalTrade.Ordering.Entities.Entities;

[Table(Name = "order_items", Schema = "ordering")]
public class OrderItemEntity
{
    [PrimaryKey, Identity, Column("id")] public long Id { get; set; }

    [Column("order_id"), NotNull] public long OrderId { get; set; }

    [Column("product_id"), NotNull] public long ProductId { get; set; }

    [Column("quantity"), NotNull] public long Quantity { get; set; }
}
