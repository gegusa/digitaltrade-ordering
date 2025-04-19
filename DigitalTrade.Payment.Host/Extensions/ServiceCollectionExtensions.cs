using DigitalTrade.Payment.Api.Contracts.Payment;
using DigitalTrade.Payment.AppServices.Kafka;
using DigitalTrade.Payment.AppServices.Options;
using KafkaFlow;
using KafkaFlow.Serializer;

namespace DigitalTrade.Payment.Host.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaFlow(this IServiceCollection services, IConfiguration configuration)
    {
        var kafkaOptions = configuration
                               .GetSection(KafkaOptions.Section)
                               .Get<KafkaOptions>()
                           ?? throw new ArgumentNullException();

        if (kafkaOptions.Servers.Length == 0)
        {
            throw new ArgumentException("kafkaOptions.Servers.Length == 0");
        }

        if (kafkaOptions.ConsumerGroup is null)
        {
            throw new ArgumentNullException(kafkaOptions.ConsumerGroup);
        }

        return services.AddKafka(kafka => kafka
            .UseConsoleLog()
            .AddCluster(cluster => cluster
                .WithBrokers(kafkaOptions.Servers)
                .CreateTopicIfNotExists(Topics.PaymentRequest, 1, 1)
                .CreateTopicIfNotExists(Topics.OrderRequest, 1, 1)
                .AddConsumer(consumer => consumer
                    .Topic("orders")
                    .WithGroupId(kafkaOptions.ConsumerGroup)
                    .WithBufferSize(100)
                    .WithWorkersCount(10)
                    .AddMiddlewares(m => m
                        .AddDeserializer<JsonCoreDeserializer>()
                        .AddTypedHandlers(h => h
                            .WithHandlerLifetime(InstanceLifetime.Scoped)
                            .AddHandler<OrderCreatedHandler>()
                            .WhenNoHandlerFound(context =>
                                Console.WriteLine("Message not handled > Partition: {0} | Offset: {1}",
                                    context.ConsumerContext.Partition,
                                    context.ConsumerContext.Offset
                                )
                            )
                        )
                    )
                )
                .AddProducer(
                    OrderCreatedHandler.ProducerName,
                    producer => producer
                        .DefaultTopic(Topics.PaymentRequest)
                        .AddMiddlewares(m =>
                            m.AddSerializer<JsonCoreSerializer>()
                        )
                )
            )
        );
    }
}