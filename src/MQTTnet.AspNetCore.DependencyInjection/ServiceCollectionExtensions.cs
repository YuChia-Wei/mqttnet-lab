using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MQTTnet.Client;

namespace MQTTnet.AspNetCore.DependencyInjection;

public static class ServiceCollectionExtensions
{
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

    private static IServiceCollection AddMqttClientOptions(
        this IServiceCollection serviceCollection,
        Action<IServiceProvider, MqttClientOptionsBuilder>? optionsAction)
    {
        serviceCollection.TryAddSingleton<MqttClientOptions>(serviceProvider =>
                                                                 CreateMqttClientOptions(
                                                                     serviceProvider, optionsAction));
        return serviceCollection;
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