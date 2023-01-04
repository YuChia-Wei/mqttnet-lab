using Microsoft.Extensions.DependencyInjection.Extensions;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace mqttnet.publisher;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddManagedMqttBackgroundConnectService(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<ManagedMqttClientBackgroundService>();
        return serviceCollection;
    }

    public static IServiceCollection AddManagedMqttClient(
        this IServiceCollection serviceCollection,
        Action<IServiceProvider, ManagedMqttClientOptionsBuilder>? optionsAction)
    {
        serviceCollection.TryAddSingleton(new MqttFactory());
        serviceCollection.TryAddSingleton<IMqttClient>(provider => provider.GetRequiredService<MqttFactory>()
                                                                           .CreateMqttClient());
        AddManagedMqttClientOptions(serviceCollection, optionsAction);

        return serviceCollection;
    }

    public static IServiceCollection AddMqttBackgroundConnectService(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<MqttClientBackgroundService>();
        return serviceCollection;
    }

    public static IServiceCollection AddMqttClient(
        this IServiceCollection serviceCollection,
        Action<IServiceProvider, MqttClientOptionsBuilder>? optionsAction)
    {
        serviceCollection.TryAddSingleton(new MqttFactory());
        serviceCollection.TryAddSingleton<IMqttClient>(provider => provider.GetRequiredService<MqttFactory>()
                                                                           .CreateMqttClient());
        AddMqttClientOptions(serviceCollection, optionsAction);

        return serviceCollection;
    }

    private static IServiceCollection AddManagedMqttClientOptions(
        this IServiceCollection serviceCollection,
        Action<IServiceProvider, ManagedMqttClientOptionsBuilder>? optionsAction)
    {
        serviceCollection.TryAddSingleton<ManagedMqttClientOptions>(serviceProvider =>
                                                                        CreateManagedMqttClientOptions(
                                                                            serviceProvider, optionsAction));
        return serviceCollection;
    }

    private static IServiceCollection AddMqttClientOptions(
        this IServiceCollection serviceCollection,
        Action<IServiceProvider, MqttClientOptionsBuilder>? optionsAction)
    {
        serviceCollection.TryAddSingleton<MqttClientOptions>(serviceProvider =>
                                                                 CreateMqttClientOptions(
                                                                     serviceProvider, optionsAction));
        return serviceCollection;
    }

    private static ManagedMqttClientOptions CreateManagedMqttClientOptions(
        this IServiceProvider applicationServiceProvider,
        Action<IServiceProvider, ManagedMqttClientOptionsBuilder>? optionsAction)
    {
        var builder = new ManagedMqttClientOptionsBuilder();

        optionsAction?.Invoke(applicationServiceProvider, builder);

        return builder.Build();
    }

    private static MqttClientOptions CreateMqttClientOptions(
        IServiceProvider applicationServiceProvider,
        Action<IServiceProvider, MqttClientOptionsBuilder>? optionsAction)
    {
        var builder = new MqttClientOptionsBuilder();

        optionsAction?.Invoke(applicationServiceProvider, builder);

        return builder.Build();
    }
}