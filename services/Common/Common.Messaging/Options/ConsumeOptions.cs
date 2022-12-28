namespace Common.Messaging.Options;

public class ConsumeOptions
{
    public required string ExchangeName { get; set; }
    
    public required ExchangeType ExchangeType { get; set; }
    
    public required string QueueName { get; set; }

    public required string RoutingKey { get; set; }

    public bool SequentialFetch { get; set; } = true;
    
    public bool AutoAcknowledge { get; set; } = false;
}
