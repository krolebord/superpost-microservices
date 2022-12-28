namespace Common.Messaging.Options;

public class ProduceOptions
{
    public required string ExchangeName { get; set; }
    
    public required ExchangeType ExchangeType { get; set; }
    
    public required string RoutingKey { get; set; }

    public bool Mandatory { get; set; } = false;
}
