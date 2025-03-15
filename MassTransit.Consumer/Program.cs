using MassTransit.Consumer.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MassTransit.Consumer;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    // Register the consumer
                    x.AddConsumer<SubmitOrderConsumer>();
                    x.SetSnakeCaseEndpointNameFormatter();

                    // Configure RabbitMQ
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        var settings = hostContext.Configuration.GetSection(nameof(MassTransitSettings)).Get<MassTransitSettings>()!;

                        cfg.Host(settings.Host, settings.Port, settings.VHost, h =>
                        {
                            h.Username(settings.Username);
                            h.Password(settings.Password);
                        });

                        cfg.ReceiveEndpoint("submit-order-v2", e =>
                        {
                            e.SetQuorumQueue(settings.ClusterSize);
                            e.ConfigureConsumer<SubmitOrderConsumer>(context);
                        });
                    });
                });
            });

        var host = builder.Build();
        await host.RunAsync();
    }
}
