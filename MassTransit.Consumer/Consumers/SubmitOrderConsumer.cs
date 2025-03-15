using MassTransit.Contracts;
using MassTransit.Transports;
using Microsoft.Extensions.Logging;

namespace MassTransit.Consumer.Consumers;

public class SubmitOrderConsumer : IConsumer<SubmitOrder>
{
    private readonly ILogger<SubmitOrderConsumer> _logger;

    public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SubmitOrder> context)
    {
        _logger.LogInformation("Consumer started: {Name}", nameof(SubmitOrder));

        await Task.Delay(2000);
        _logger.LogInformation("Order received {Id} {Name}", context.Message.Id, context.Message.Name);

        _logger.LogInformation("Consumer finished: {Name}", nameof(SubmitOrder));
    }
}
