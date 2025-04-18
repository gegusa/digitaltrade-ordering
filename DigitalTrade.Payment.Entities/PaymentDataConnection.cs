using DigitalTrade.Payment.Entities.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace DigitalTrade.Payment.Entities;

/// <summary>
/// Абстракция подключения к базе данных.
/// </summary>
public class PaymentDataConnection : DataConnection
{
    public PaymentDataConnection(DataOptions<PaymentDataConnection> options)
        : base(options.Options)
    {
    }

    /// <summary>
    /// Таблица Запросы об оплате.
    /// </summary>
    public ITable<PaymentEntity> Clients => this.GetTable<PaymentEntity>();
}