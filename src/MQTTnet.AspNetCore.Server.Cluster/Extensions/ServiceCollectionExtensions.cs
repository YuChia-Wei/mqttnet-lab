using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MQTTnet.AspNetCore.Server.Cluster.BackgroundServices;
using MQTTnet.AspNetCore.Server.Cluster.Events;
using MQTTnet.AspNetCore.Server.Cluster.Infrastructure;
using MQTTnet.AspNetCore.Server.Cluster.Options;
using StackExchange.Redis;

namespace MQTTnet.AspNetCore.Server.Cluster.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMqttClusterQueueRedisDb(
        this IServiceCollection serviceCollection,
        Action<MqttClusterQueueDatabaseOptions> action)
    {
        var clusterQueueDatabaseOptions = new MqttClusterQueueDatabaseOptions();
        action.Invoke(clusterQueueDatabaseOptions);

        serviceCollection.TryAddSingleton<IMqttClusterQueueDatabase, MqttClusterQueueDatabase>();
        serviceCollection.TryAddSingleton<InterceptingPublishEvents>();
        serviceCollection.TryAddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(clusterQueueDatabaseOptions.RedisConnectionString));
        serviceCollection.AddHostedService<RedisMessagingBackgroundService>();

        return serviceCollection;
    }
}