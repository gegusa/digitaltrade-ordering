using DigitalTrade.Ordering.Entities.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace DigitalTrade.Ordering.Entities;

/// <summary>
/// Абстракция подключения к базе данных.
/// </summary>
public class OrderingDataConnection : DataConnection
{
    public OrderingDataConnection(DataOptions<OrderingDataConnection> options)
        : base(options.Options)
    {
    }

    /// <summary>
    /// Таблица Запросы об оплате.
    /// </summary>
    public ITable<OrderEntity> Orders => this.GetTable<OrderEntity>();
}