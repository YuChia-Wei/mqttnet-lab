using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore.Server.Internal;
using MQTTnet.Server;

namespace MQTTnet.AspNetCore.Server;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMqttServerEventHandler(
        this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }

    /// <summary>
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="setupAction"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static IMqttServerBuilder AddMqttServer(
        this IServiceCollection serviceCollection,
        Action<MqttServerOptionsBuilder> setupAction = null!)
    {
        if (serviceCollection is null)
        {
            throw new ArgumentNullException(nameof(serviceCollection));
        }

        AspNetCore.ServiceCollectionExtensions.AddMqttServer(serviceCollection, setupAction);
        // AddMqttServer = 以下兩行
        // builder.Services.AddMqttConnectionHandler();
        // builder.Services.AddHostedMqttServer();

        serviceCollection.AddConnections();

        return new MqttServerBuilder(serviceCollection);
    }
}