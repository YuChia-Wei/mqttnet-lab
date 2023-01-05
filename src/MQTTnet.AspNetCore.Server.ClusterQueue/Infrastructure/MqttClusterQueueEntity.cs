using System;
using System.Text.Json;
using MQTTnet.Server;
using StackExchange.Redis;

namespace MQTTnet.AspNetCore.Server.ClusterQueue.Infrastructure;

/// <summary>
/// Mqtt Cluster Sync Data Entity
/// </summary>
internal record MqttClusterQueueEntity
{
    private static string _clusterSyncTopicSuffixes = "_sync";

    /// <summary>
    /// 原始發送人
    /// </summary>
    public string OriginPublisher { get; set; }

    /// <summary>
    /// 原始 Broker (用於判斷是否為自己發的資料)
    /// </summary>
    public string OriginBroker { get; set; }

    /// <summary>
    /// 需要轉發到其他 Broker 的 Message
    /// </summary>
    public MqttApplicationMessage ApplicationMessage { get; set; }

    public InjectedMqttApplicationMessage CreateInjectedMqttApplicationMessage()
    {
        this.ApplicationMessage.Topic += _clusterSyncTopicSuffixes;

        var injectedMqttApplicationMessage = new InjectedMqttApplicationMessage(this.ApplicationMessage) { SenderClientId = this.OriginPublisher };
        return injectedMqttApplicationMessage;
    }

    public static MqttClusterQueueEntity? Deserialize(RedisValue message)
    {
        var mqttSyncData = JsonSerializer.Deserialize<MqttClusterQueueEntity>(message);
        return mqttSyncData;
    }

    public static bool IsClusterSyncTopic(InterceptingPublishEventArgs eventArgs)
    {
        return eventArgs.ApplicationMessage.Topic.EndsWith(_clusterSyncTopicSuffixes);
    }

    public bool IsSameBroker()
    {
        return this.OriginBroker == Environment.MachineName;
    }

    public static MqttClusterQueueEntity ParseToMqttClusterSyncEntity(InterceptingPublishEventArgs eventArgs)
    {
        var mqttSyncData = new MqttClusterQueueEntity
        {
            OriginPublisher = eventArgs.ClientId,
            OriginBroker = Environment.MachineName,
            ApplicationMessage = eventArgs.ApplicationMessage
        };
        return mqttSyncData;
    }

    public static string RevertToOriginTopic(InterceptingPublishEventArgs eventArgs)
    {
        return eventArgs.ApplicationMessage.Topic.Replace(_clusterSyncTopicSuffixes, "");
    }
}