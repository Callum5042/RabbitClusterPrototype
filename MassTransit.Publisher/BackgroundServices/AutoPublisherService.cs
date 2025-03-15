using MassTransit.Contracts;
using MassTransit.Transports;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MassTransit.Publisher.BackgroundServices;

public class AutoPublisherService : BackgroundService
{
    private readonly ILogger<AutoPublisherService> _logger;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public AutoPublisherService(ILogger<AutoPublisherService> logger, ISendEndpointProvider sendEndpointProvider)
    {
        _logger = logger;
        _sendEndpointProvider = sendEndpointProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Message published {BackgroundService}", nameof(AutoPublisherService));

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:submit-order-v2"));
            await endpoint.Send(new SubmitOrder
            {
                Id = Guid.NewGuid(),
                Name = "Pineapple Pizza",
            });

            await Task.Delay(5000);
        }
    }
}
