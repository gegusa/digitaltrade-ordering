using LinqToDB.Mapping;

namespace DigitalTrade.Ordering.Entities.Entities;

[Table(Name = "products", Schema = "ordering")]
public class ProductEntity
{
    [PrimaryKey, Identity, Column("id")] public long Id { get; set; }

    [Column("name"), NotNull] public string Name { get; set; }

    [Column("price"), NotNull] public decimal Price { get; set; }

    [Association(ThisKey = nameof(Id), OtherKey = nameof(OrderItemEntity.ProductId), CanBeNull = false)]
    public IReadOnlyList<OrderItemEntity> OrderItems { get; set; } = [];
}