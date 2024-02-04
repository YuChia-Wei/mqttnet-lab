using Microsoft.Extensions.Logging;
using MQTTnet.AspNetCore.Server.@ref.Entities;
using MQTTnet.Server;

namespace MQTTnet.AspNetCore.Server.@ref.EventHandlers;

internal sealed class InterceptingPublishEvents
{
    private readonly ILogger<InterceptingPublishEvents> _logger;
    private readonly RedisQueuePublisher _redisQueuePublisher;

    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public InterceptingPublishEvents(ILogger<InterceptingPublishEvents> logger, RedisQueuePublisher redisQueuePublisher)
    {
        this._logger = logger;
        this._redisQueuePublisher = redisQueuePublisher;
    }

    /// <summary>
    /// 攔截發佈事件
    /// </summary>
    /// <param name="eventArgs"></param>
    /// <returns></returns>
    public Task PublishToRedisQueueAsync(InterceptingPublishEventArgs eventArgs)
    {
        this._logger.LogInformation(
            $"Client '{eventArgs.ClientId}' publish: {eventArgs.ApplicationMessage.Topic}:{eventArgs.ApplicationMessage.ConvertPayloadToString()}");

        if (MqttClusterQueueEntity.IsClusterSyncTopic(eventArgs))
        {
            eventArgs.ApplicationMessage.Topic = MqttClusterQueueEntity.RevertToOriginTopic(eventArgs);
            this._logger.LogInformation(
                $"Get Other Broker Message : Client '{eventArgs.ClientId}' publish: {eventArgs.ApplicationMessage.Topic}:{eventArgs.ApplicationMessage.ConvertPayloadToString()}");
            return Task.CompletedTask;
        }

        return this._redisQueuePublisher.PublishAsync(eventArgs);
    }
}