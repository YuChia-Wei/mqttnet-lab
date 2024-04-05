using Microsoft.Extensions.Logging;
using MQTTnet.AspNetCore.Server.@ref.Options;
using MQTTnet.Server;

namespace MQTTnet.AspNetCore.Server.@ref.EventHandlers;

/// <summary>
/// Mqtt server event handler
/// </summary>
public class MqttServerEventLogger : IMqttServerEventLogger
{
    private readonly ILogger<MqttServerEventLogger> _logger;
    private readonly LogLevel _loggingLevel;

    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public MqttServerEventLogger(ILogger<MqttServerEventLogger> logger, MqttEventLogOptions options)
    {
        this._logger = logger;
        this._loggingLevel = options.DefaultLogLevel;
    }

    public Task OnClientConnectedAsync(ClientConnectedEventArgs eventArgs)
    {
        this._logger.Log(this._loggingLevel, $"Client '{eventArgs.ClientId}' connected.");
        return Task.CompletedTask;
    }

    public Task OnClientDisconnectedAsync(ClientDisconnectedEventArgs eventArgs)
    {
        this._logger.Log(this._loggingLevel, $"Client '{eventArgs.ClientId}' disconnected.");
        return Task.CompletedTask;
    }

    public Task OnInterceptingPublishAsync(InterceptingPublishEventArgs eventArgs)
    {
        this._logger.Log(this._loggingLevel, $"Client '{eventArgs.ClientId}' wants to connect. Accepting!");
        return Task.CompletedTask;
    }

    public Task ValidateConnectionAsync(ValidatingConnectionEventArgs eventArgs)
    {
        this._logger.Log(this._loggingLevel, $"Client '{eventArgs.ClientId}' wants to connect. Accepting!");
        return Task.CompletedTask;
    }
}