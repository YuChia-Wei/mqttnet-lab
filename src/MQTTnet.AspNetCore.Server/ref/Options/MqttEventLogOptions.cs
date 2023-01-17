using Microsoft.Extensions.Logging;

namespace MQTTnet.AspNetCore.Server.@ref.Options;

public class MqttEventLogOptions
{
    public LogLevel DefaultLogLevel { get; set; } = LogLevel.Debug;
}