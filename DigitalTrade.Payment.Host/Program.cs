using DigitalTrade.Payment.Api.Contracts.Payment;
using KafkaFlow;
using KafkaFlow.Serializer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddKafka(kafka => kafka
    .UseConsoleLog()
    .AddCluster(cluster => cluster
        .WithBrokers(new[] { "localhost:9092" })
        .CreateTopicIfNotExists(Topics.PaymentRequest, 1, 1)
        .AddConsumer(consumer => consumer
            .Topic("orders")
            .WithGroupId("payment-service")
            .WithName("order-created-consumer")
            .WithBufferSize(100)
            .WithWorkersCount(4)
            .AddMiddlewares(m => m
                .AddSerializer<JsonCoreSerializer>()
                .AddTypedHandlers(h => h.AddHandler<PaymentMessageConsumer>())
            )
        )
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();