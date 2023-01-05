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

    public Task OnClientConnected(ClientConnectedEventArgs eventArgs)
    {
        this._logger.LogInformation($"Client '{eventArgs.ClientId}' connected.");
        return Task.CompletedTask;
    }

    public Task OnClientDisconnected(ClientDisconnectedEventArgs eventArgs)
    {
        this._logger.LogInformation($"Client '{eventArgs.ClientId}' disconnected.");
        return Task.CompletedTask;
    }

    public Task ValidateConnection(ValidatingConnectionEventArgs eventArgs)
    {
        this._logger.LogInformation($"Client '{eventArgs.ClientId}' wants to connect. Accepting!");
        return Task.CompletedTask;
    }
}