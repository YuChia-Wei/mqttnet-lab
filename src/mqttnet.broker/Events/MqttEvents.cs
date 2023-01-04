using System.Text.Json;
using MQTTnet;
using MQTTnet.Server;
using StackExchange.Redis;

namespace mqttnet.broker.Events;

internal sealed class MqttEvents
{
    private IConnectionMultiplexer _connectionMultiplexer;

    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public MqttEvents(IConnectionMultiplexer connectionMultiplexer)
    {
        this._connectionMultiplexer = connectionMultiplexer;
    }

    public Task OnClientConnected(ClientConnectedEventArgs eventArgs)
    {
        Console.WriteLine($"Client '{eventArgs.ClientId}' connected.");
        return Task.CompletedTask;
    }

    public Task OnClientDisconnected(ClientDisconnectedEventArgs eventArgs)
    {
        Console.WriteLine($"Client '{eventArgs.ClientId}' disconnected.");
        return Task.CompletedTask;
    }

    public Task OnInterceptingPublish(InterceptingPublishEventArgs eventArgs)
    {
        Console.WriteLine(
            $"Client '{eventArgs.ClientId}' publish: {eventArgs.ApplicationMessage.Topic}:{eventArgs.ApplicationMessage.ConvertPayloadToString()}");
        
        if (eventArgs.ApplicationMessage.Topic.EndsWith("_sync"))
        {
            eventArgs.ApplicationMessage.Topic = eventArgs.ApplicationMessage.Topic.Replace("_sync", "");
            Console.WriteLine(
                $"Pass : Client '{eventArgs.ClientId}' publish: {eventArgs.ApplicationMessage.Topic}:{eventArgs.ApplicationMessage.ConvertPayloadToString()}");
            return Task.CompletedTask;
        }

        var mqttSyncData = new MqttSyncData()
        {
            OriginPublisher = eventArgs.ClientId,
            OriginBroker = Environment.GetEnvironmentVariable("host"),
            ApplicationMessage = eventArgs.ApplicationMessage
        };

        var serialize = JsonSerializer.Serialize(mqttSyncData);

        return this._connectionMultiplexer.GetSubscriber()
                   .PublishAsync($"{AppDomain.CurrentDomain.FriendlyName}:MqttSync", serialize);
    }

    public Task ValidateConnection(ValidatingConnectionEventArgs eventArgs)
    {
        Console.WriteLine($"Client '{eventArgs.ClientId}' wants to connect. Accepting!");
        return Task.CompletedTask;
    }
}

internal record MqttSyncData
{
    /// <summary>
    /// 原始發送人
    /// </summary>
    public string OriginPublisher { get; set; }

    /// <summary>
    /// 原始 Broker
    /// </summary>
    public string OriginBroker { get; set; }

    /// <summary>
    /// 需要轉發到其他 Broker 的 Message
    /// </summary>
    public MqttApplicationMessage ApplicationMessage { get; set; }

    public bool IsSameBroker()
    {
        return this.OriginBroker == Environment.GetEnvironmentVariable("host");
    }
}