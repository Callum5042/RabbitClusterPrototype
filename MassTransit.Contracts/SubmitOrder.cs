namespace MassTransit.Contracts;

public record SubmitOrder
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
}
