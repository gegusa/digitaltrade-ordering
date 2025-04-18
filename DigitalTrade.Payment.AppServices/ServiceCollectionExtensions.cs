using Microsoft.Extensions.DependencyInjection;
using DigitalTrade.Payment.Api.Contracts.Payment;
using DigitalTrade.Payment.AppServices.Kafka;
using KafkaFlow;
using KafkaFlow.Serializer;

namespace DigitalTrade.Payment.AppServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafka(this IServiceCollection services)
    {
        return services.AddKafka(kafka => kafka
            .UseConsoleLog()
            .AddCluster(cluster => cluster
                .WithBrokers(["localhost:9092"])
                .CreateTopicIfNotExists(Topics.PaymentRequest, 1, 1)
                .CreateTopicIfNotExists(Topics.OrderRequest, 1, 1)
                .AddConsumer(consumer => consumer
                    .Topic("orders")
                    .WithGroupId("payment-service")
                    .WithName("order-created-consumer")
                    .WithBufferSize(100)
                    .WithWorkersCount(10)
                    .AddMiddlewares(m => m
                        .AddDeserializer<JsonCoreDeserializer>()
                        .AddTypedHandlers(h => h.AddHandler<OrderCreatedHandler>())
                    )
                )
                .AddProducer(
                    "producerName",
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