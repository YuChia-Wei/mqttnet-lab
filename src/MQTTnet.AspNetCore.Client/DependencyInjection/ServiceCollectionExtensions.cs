using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MQTTnet.AspNetCore.Client.BackgroundServices;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace MQTTnet.AspNetCore.Client.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddManagedMqttClient(
        this IServiceCollection serviceCollection,
        Action<IServiceProvider, ManagedMqttClientOptionsBuilder> optionsAction)
    {
        serviceCollection.TryAddSingleton(new MqttFactory());

        serviceCollection.TryAddSingleton<IManagedMqttClient>(provider =>
        {
            return provider.GetRequiredService<MqttFactory>()
                           .CreateManagedMqttClient();
        });

        serviceCollection.TryAddSingleton<ManagedMqttClientOptions>(serviceProvider =>
        {
            return InvokeManagedMqttClientOptions(
                serviceProvider, optionsAction);
        });

        return serviceCollection;
    }

    public static IServiceCollection AddManagedMqttClientBackgroundService(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<ManagedMqttClientBackgroundService>();
        return serviceCollection;
    }

    public static IServiceCollection AddMqttClient(
        this IServiceCollection serviceCollection,
        Action<IServiceProvider, MqttClientOptionsBuilder> optionsAction)
    {
        serviceCollection.TryAddSingleton(new MqttFactory());
        serviceCollection.TryAddSingleton<IMqttClient>(provider =>
        {
            return provider.GetRequiredService<MqttFactory>()
                           .CreateMqttClient();
        });
        serviceCollection.TryAddSingleton<MqttClientOptions>(serviceProvider =>
        {
            return InvokeMqttClientOptions(serviceProvider, optionsAction);
        });
        return serviceCollection;
    }

    public static IServiceCollection AddMqttClientBackgroundService(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<MqttClientBackgroundService>();
        return serviceCollection;
    }

    private static ManagedMqttClientOptions InvokeManagedMqttClientOptions(
        this IServiceProvider serviceProvider,
        Action<IServiceProvider, ManagedMqttClientOptionsBuilder> optionsAction)
    {
        var builder = new ManagedMqttClientOptionsBuilder();

        optionsAction.Invoke(serviceProvider, builder);

        return builder.Build();
    }

    private static MqttClientOptions InvokeMqttClientOptions(
        IServiceProvider applicationServiceProvider,
        Action<IServiceProvider, MqttClientOptionsBuilder> optionsAction)
    {
        var builder = new MqttClientOptionsBuilder();

        optionsAction.Invoke(applicationServiceProvider, builder);

        return builder.Build();
    }
}