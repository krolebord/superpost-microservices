namespace Common.Messaging.Options;

public enum ExchangeType
{
    Direct,
    Topic
}

public static class ExchangeTypeExtensions
{
    public static string GetValue(this ExchangeType type)
    {
        return type switch
        {
            ExchangeType.Direct => "direct",
            ExchangeType.Topic => "topic",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
