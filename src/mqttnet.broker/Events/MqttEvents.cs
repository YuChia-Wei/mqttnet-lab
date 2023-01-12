using MQTTnet.Server;

namespace mqttnet.broker.Events;

internal sealed class MqttEvents
{
    private readonly ILogger<MqttEvents> _logger;

    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public MqttEvents(ILogger<MqttEvents> logger)
    {
        this._logger = logger;
    }

    public Task OnClientConnectedAsync(ClientConnectedEventArgs eventArgs)
    {
        this._logger.Log(LogLevel.Information, $"Client '{eventArgs.ClientId}' connected.");
        return Task.CompletedTask;
    }

    public Task OnClientDisconnectedAsync(ClientDisconnectedEventArgs eventArgs)
    {
        this._logger.Log(LogLevel.Information, $"Client '{eventArgs.ClientId}' disconnected.");
        return Task.CompletedTask;
    }

    public Task ValidateConnectionAsync(ValidatingConnectionEventArgs eventArgs)
    {
        this._logger.Log(LogLevel.Information, $"Client '{eventArgs.ClientId}' wants to connect. Accepting!");
        return Task.CompletedTask;
    }
}