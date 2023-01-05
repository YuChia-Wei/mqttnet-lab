using System;
using System.Text.Json;
using System.Threading.Tasks;
using MQTTnet.Server;
using StackExchange.Redis;

namespace MQTTnet.AspNetCore.Server.ClusterQueue.Infrastructure;

/// <summary>
/// Mqtt Cluster Queue Database
/// </summary>
internal sealed class MqttClusterQueueDatabase : IMqttClusterQueueDatabase
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public MqttClusterQueueDatabase(IConnectionMultiplexer connectionMultiplexer)
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