using System.Text.Json;
using MQTTnet.AspNetCore.Server.@ref.Entities;
using MQTTnet.Server;
using StackExchange.Redis;

namespace MQTTnet.AspNetCore.Server.@ref;

/// <summary>
/// Mqtt Cluster Queue Database
/// </summary>
internal sealed class RedisQueuePublisher
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public RedisQueuePublisher(IConnectionMultiplexer connectionMultiplexer)
    {
        this._connectionMultiplexer = connectionMultiplexer;
    }

    /// <summary>
    /// Publish to other broker
    /// </summary>
    /// <param name="eventArgs"></param>
    /// <returns></returns>
    public Task PublishAsync(InterceptingPublishEventArgs eventArgs)
    {
        var mqttSyncData = MqttClusterQueueEntity.ParseToMqttClusterSyncEntity(eventArgs);

        var serialize = JsonSerializer.Serialize(mqttSyncData);

        return this._connectionMultiplexer.GetSubscriber()
                   .PublishAsync($"{AppDomain.CurrentDomain.FriendlyName}:MqttSync", serialize);
    }
}