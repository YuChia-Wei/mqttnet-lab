using Microsoft.Extensions.Logging;
using MQTTnet.AspNetCore.Server.Cluster.Infrastructure;
using MQTTnet.Server;

namespace MQTTnet.AspNetCore.Server.Cluster.Events;

internal sealed class InterceptingPublishEvents
{
    private readonly ILogger<InterceptingPublishEvents> _logger;
    private readonly IMqttQueueDatabase _mqttQueueDatabase;

    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public InterceptingPublishEvents(ILogger<InterceptingPublishEvents> logger, IMqttQueueDatabase mqttQueueDatabase)
    {
        this._logger = logger;
        this._mqttQueueDatabase = mqttQueueDatabase;
    }

    /// <summary>
    /// 攔截發佈事件
    /// </summary>
    /// <param name="eventArgs"></param>
    /// <returns></returns>
    public Task PublishToRedisAsync(InterceptingPublishEventArgs eventArgs)
    {
        this._logger.LogInformation(
            $"Client '{eventArgs.ClientId}' publish: {eventArgs.ApplicationMessage.Topic}:{eventArgs.ApplicationMessage.ConvertPayloadToString()}");

        if (MqttRedisQueueEntity.IsClusterSyncTopic(eventArgs))
        {
            eventArgs.ApplicationMessage.Topic = MqttRedisQueueEntity.RevertToOriginTopic(eventArgs);
            this._logger.LogInformation(
                $"Get Other Broker Message : Client '{eventArgs.ClientId}' publish: {eventArgs.ApplicationMessage.Topic}:{eventArgs.ApplicationMessage.ConvertPayloadToString()}");
            return Task.CompletedTask;
        }

        return this._mqttQueueDatabase.PublishAsync(eventArgs);
    }
}