using DigitalTrade.Ordering.AppServices.Kafka;
using DigitalTrade.Ordering.AppServices.Options;
using KafkaFlow;
using KafkaFlow.Serializer;
using Topics = DigitalTrade.Ordering.Api.Contracts.Kafka.Topics;

namespace DigitalTrade.Ordering.Host.Extensions;

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
                .CreateTopicIfNotExists(Topics.PaymentRequestedTopicName, 1, 1)
                .AddConsumer(consumer => consumer
                    .Topic(Basket.Api.Contracts.Kafka.Topics.BasketCheckoutRequestedName)
                    .WithGroupId(kafkaOptions.ConsumerGroup)
                    .WithBufferSize(100)
                    .WithWorkersCount(10)
                    .AddMiddlewares(m => m
                        .AddDeserializer<JsonCoreDeserializer>()
                        .AddTypedHandlers(h => h
                            .WithHandlerLifetime(InstanceLifetime.Scoped)
                            .AddHandler<BasketCheckoutRequestedEventHandler>()
                            .WhenNoHandlerFound(context =>
                                Console.WriteLine("Message not handled > Partition: {0} | Offset: {1}",
                                    context.ConsumerContext.Partition,
                                    context.ConsumerContext.Offset
                                )
                            )
                        )
                    )
                )
                .AddConsumer(consumer => consumer
                    .Topic(Topics.PaymentRequestedTopicName)
                    .WithGroupId(kafkaOptions.ConsumerGroup)
                    .WithBufferSize(100)
                    .WithWorkersCount(10)
                    .AddMiddlewares(m => m
                        .AddDeserializer<JsonCoreDeserializer>()
                        .AddTypedHandlers(h => h
                            .WithHandlerLifetime(InstanceLifetime.Scoped)
                            .AddHandler<PaymentRequestedEventHandler>()
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
                    Topics.PaymentRequestedTopicProducerName,
                    producer => producer
                        .DefaultTopic(Topics.PaymentRequestedTopicName)
                        .AddMiddlewares(m =>
                            m.AddSerializer<JsonCoreSerializer>()
                        )
                )
            )
        );
    }
}