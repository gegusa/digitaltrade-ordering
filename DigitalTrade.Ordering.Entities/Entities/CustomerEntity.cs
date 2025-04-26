using LinqToDB.Mapping;

namespace DigitalTrade.Ordering.Entities.Entities;

[Table(Name = "customers", Schema = "ordering")]
public class CustomerEntity
{
    [PrimaryKey, Identity, Column("id")] public long Id { get; set; }

    [Column("name"), NotNull] public string Name { get; set; } = null!;

    [Column("email"), NotNull] public string Email { get; set; } = null!;

    [Association(ThisKey = nameof(Id), OtherKey = nameof(OrderEntity.CustomerId), CanBeNull = false)]
    public IReadOnlyList<OrderEntity> Orders { get; set; } = [];
}