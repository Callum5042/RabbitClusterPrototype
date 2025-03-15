using MassTransit.Consumer;
using MassTransit.Publisher.BackgroundServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MassTransit.Publisher;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    // Configure RabbitMQ
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        var settings = hostContext.Configuration.GetSection(nameof(MassTransitSettings)).Get<MassTransitSettings>()!;

                        cfg.Host(settings.Host, settings.Port, settings.VHost, h =>
                        {
                            h.Username(settings.Username);
                            h.Password(settings.Password);
                        });
                    });
                });

                services.AddHostedService<AutoPublisherService>();
            });

        var host = builder.Build();
        await host.RunAsync();
    }
}
