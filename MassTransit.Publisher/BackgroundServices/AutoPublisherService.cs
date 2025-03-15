using MassTransit.Contracts;
using MassTransit.Transports;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MassTransit.Publisher.BackgroundServices;

public class AutoPublisherService : BackgroundService
{
    private readonly ILogger<AutoPublisherService> _logger;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IBusControl busControl;

    public AutoPublisherService(ILogger<AutoPublisherService> logger, ISendEndpointProvider sendEndpointProvider, IBusControl busControl)
    {
        _logger = logger;
        _sendEndpointProvider = sendEndpointProvider;
        this.busControl = busControl;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Message published {BackgroundService}", nameof(AutoPublisherService));

            // Apprently doesn't work properly with quorum queues
            // https://stackoverflow.com/questions/71470180/creating-quorum-queues-by-sender-in-rabbitmq-by-masstransit
            //var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:submit-order"));
            //await endpoint.Send(new SubmitOrder
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Pineapple Pizza",
            //});

            await busControl.Publish(new SubmitOrder
            {
                Id = Guid.NewGuid(),
                Name = "Pineapple Pizza",
            });


            await Task.Delay(5000);
        }
    }
}
