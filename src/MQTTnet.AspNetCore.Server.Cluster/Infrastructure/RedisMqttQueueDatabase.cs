﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using MQTTnet.Server;
using StackExchange.Redis;

namespace MQTTnet.AspNetCore.Server.Cluster.Infrastructure;

/// <summary>
/// Mqtt Cluster Queue Database
/// </summary>
internal sealed class RedisMqttQueueDatabase : IMqttQueueDatabase
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public RedisMqttQueueDatabase(IConnectionMultiplexer connectionMultiplexer)
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
        var mqttSyncData = MqttRedisQueueEntity.ParseToMqttClusterSyncEntity(eventArgs);

        var serialize = JsonSerializer.Serialize(mqttSyncData);

        return this._connectionMultiplexer.GetSubscriber()
                   .PublishAsync($"{AppDomain.CurrentDomain.FriendlyName}:MqttSync", serialize);
    }
}