using MQTTnet.Server;

namespace MQTTnet.AspNetCore.Server.@ref.EventHandlers;

public interface IMqttServerEventLogger
{
    Task OnClientConnectedAsync(ClientConnectedEventArgs eventArgs);
    Task OnClientDisconnectedAsync(ClientDisconnectedEventArgs eventArgs);
    Task OnInterceptingPublishAsync(InterceptingPublishEventArgs eventArgs);
    Task ValidateConnectionAsync(ValidatingConnectionEventArgs eventArgs);
}