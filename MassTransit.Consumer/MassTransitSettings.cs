namespace MassTransit.Consumer;

#nullable disable

public class MassTransitSettings
{
    public string Host { get; set; }

    public ushort Port { get; set; }

    public string VHost { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public int ClusterSize { get; set; }
}
