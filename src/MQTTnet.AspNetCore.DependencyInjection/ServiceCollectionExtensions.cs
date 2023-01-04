using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace MQTTnet.AspNetCore.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddManagedMqttClient(
        this IServiceCollection serviceCollection,
        Action<IServiceProvider, ManagedMqttClientOptionsBuilder>? optionsAction)
    {
        serviceCollection.TryAddSingleton(new MqttFactory());
        serviceCollection.TryAddSingleton<IManagedMqttClient>(provider => provider.GetRequiredService<MqttFactory>()
                                                                           .CreateManagedMqttClient());
        AddManagedMqttClientOptions(serviceCollection, optionsAction);

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