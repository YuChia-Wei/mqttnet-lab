using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MQTTnet.AspNetCore.Server.@ref.BackgroundServices;
using MQTTnet.AspNetCore.Server.@ref.EventHandlers;
using MQTTnet.AspNetCore.Server.@ref.Options;
using MQTTnet.Server;
using StackExchange.Redis;

namespace MQTTnet.AspNetCore.Server.@ref.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 加入基本的 Mqtt Event logger
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddMqttEventLogger(
        this IServiceCollection serviceCollection,
        Action<MqttEventLogOptions> action)
    {
        var mqttEventLogOptions = new MqttEventLogOptions();
        action.Invoke(mqttEventLogOptions);

        return AddLogger(serviceCollection, mqttEventLogOptions);
    }

    /// <summary>
    /// 註冊官方的 MqttServer 與需要的 Add Connections 方法，同時複寫 MqttServer 的註冊，加入事件註冊
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="queueRedisSourceOptionAction"></param>
    /// <param name="mqttServerOptionAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddMyMqttServer(
        this IServiceCollection serviceCollection,
        Action<RedisQueueOption> queueRedisSourceOptionAction = null!,
        Action<MqttServerOptionsBuilder> mqttServerOptionAction = null!)
    {
        var clusterQueueDatabaseOptions = new RedisQueueOption();
        queueRedisSourceOptionAction.Invoke(clusterQueueDatabaseOptions);

        if (IsRedisConnectionStringSettled(clusterQueueDatabaseOptions))
        {
            serviceCollection.TryAddSingleton<RedisQueuePublisher>();
            serviceCollection.TryAddSingleton<InterceptingPublishEvents>();
            serviceCollection.TryAddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(clusterQueueDatabaseOptions.RedisConnectionString!));
            serviceCollection.AddHostedService<RedisQueueBackgroundService>();
        }

        serviceCollection.AddMqttServer(mqttServerOptionAction);
        // AddMqttServer 等於以下兩行，可反組譯回去看原始碼，或是去 mqttnet 看
        // serviceCollection.AddMqttConnectionHandler();
        // serviceCollection.AddHostedMqttServer(mqttServerOptionAction);

        serviceCollection.AddConnections();

        serviceCollection.AddMqttEventLogger();

        //複寫掉前面 AddMqttServer 中註冊的 MqttServer
        serviceCollection.AddSingleton<MqttServer>(s =>
        {
            var mqttHostedServer = s.GetRequiredService<MqttHostedServer>() as MqttServer;

            var publishEvents = s.GetService<InterceptingPublishEvents>();
            if (publishEvents != null)
            {
                mqttHostedServer.InterceptingPublishAsync += publishEvents.PublishToRedisQueueAsync;
            }

            var eventHandler = s.GetService<IMqttServerEventLogger>();
            if (eventHandler != null)
            {
                mqttHostedServer.InterceptingPublishAsync += eventHandler.OnInterceptingPublishAsync;
                mqttHostedServer.ClientConnectedAsync += eventHandler.OnClientConnectedAsync;
                mqttHostedServer.ClientDisconnectedAsync += eventHandler.OnClientDisconnectedAsync;
                mqttHostedServer.ValidatingConnectionAsync += eventHandler.ValidateConnectionAsync;
            }

            return mqttHostedServer;
        });

        return serviceCollection;
    }

    private static IServiceCollection AddLogger(IServiceCollection serviceCollection, MqttEventLogOptions mqttEventLogOptions)
    {
        serviceCollection.TryAddSingleton(mqttEventLogOptions);
        serviceCollection.TryAddSingleton<IMqttServerEventLogger, MqttServerEventLogger>();
        return serviceCollection;
    }

    /// <summary>
    /// 加入基本的 Mqtt Event logger
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    private static IServiceCollection AddMqttEventLogger(
        this IServiceCollection serviceCollection)
    {
        return AddLogger(serviceCollection, new MqttEventLogOptions());
    }

    private static bool IsRedisConnectionStringSettled(RedisQueueOption clusterRedisQueueDatabaseOptions)
    {
        var isRedisConnectionStringSettled = !string.IsNullOrEmpty(clusterRedisQueueDatabaseOptions.RedisConnectionString);
        return isRedisConnectionStringSettled;
    }
}